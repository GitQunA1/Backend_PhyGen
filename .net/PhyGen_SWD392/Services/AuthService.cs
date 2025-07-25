using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Enums;
using PhyGen_SWD392.Repositories.Interface;
using PhyGen_SWD392.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PhyGen_SWD392.Services
{
    public class AuthService
    {
        private readonly IAuthRepo _authRepo;
        private readonly PhyGenDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthService(PhyGenDbContext context, IAuthRepo authRepo, IOptions<JwtSettings> jwtOptions)
        {
            _context = context;
            _authRepo = authRepo;
            _jwtSettings = jwtOptions?.Value
                ?? throw new InvalidOperationException("JWT settings not configured");
        }

        public async Task<bool> RegisterAsync(AccountRegisterDto dto)
        {
            if (dto.Password != dto.ConfirmPassword || !dto.Email.Contains('@'))
                return false;

            var existingEmail = await _authRepo.GetAccountByEmailAsync(dto.Email);
            if (existingEmail != null) return false;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var newAccount = new Account
            {
                Email = dto.Email,
                Password = hashedPassword,
                Username = dto.Email.Split('@')[0],
                EmailVerified = false,
                RoleEnum = RoleEnum.user,
                AccountTypeEnum = AccountTypeEnum.free,
                StatusEnum = AccountStatusEnum.active,
            };

            await _authRepo.AddAsync(newAccount);
            return true;
        }

        public async Task<Account?> LoginAsync(AccountLoginDto dto)
        {
            Account? account;

            if (dto.Identifier.Contains("@"))
                account = await _authRepo.GetAccountByEmailAsync(dto.Identifier);
            else
                account = await _authRepo.GetAccountByUsernameAsync(dto.Identifier);

            if (account == null ||
                account.StatusEnum != AccountStatusEnum.active ||
                account.EmailVerified != true)
                return null;

            var isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, account.Password);
            return isValidPassword ? account : null;
        }

        public string GenerateAccessToken(Account acc)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, acc.Id.ToString()),
                new Claim("id", acc.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, acc.Email),
                new Claim(ClaimTypes.Role, acc.Role ?? "user"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(Account acc)
        {
            var claims = new[]
            {
                new Claim("id", acc.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthResultDto?> LoginWithTokenAsync(AccountLoginDto dto)
        {
            var account = await LoginAsync(dto);
            if (account == null) return null;

            return new AuthResultDto
            {
                AccessToken = GenerateAccessToken(account),
                RefreshToken = GenerateRefreshToken(account),
                ExpiresIn = _jwtSettings.ExpirationMinutes
            };
        }

        public AuthResultDto? RefreshAccessToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);

                var userId = principal.FindFirst("id")?.Value;
                if (userId == null) return null;

                var account = _context.Accounts.Find(int.Parse(userId));
                if (account == null || account.StatusEnum != AccountStatusEnum.active)
                    return null;

                return new AuthResultDto
                {
                    AccessToken = GenerateAccessToken(account),
                    RefreshToken = GenerateRefreshToken(account),
                    ExpiresIn = _jwtSettings.ExpirationMinutes
                };
            }
            catch
            {
                return null;
            }
        }

        public int GetTokenExpirationMinutes() => _jwtSettings.ExpirationMinutes;
    }
}
