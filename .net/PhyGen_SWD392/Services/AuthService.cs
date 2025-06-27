using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Enums;
using PhyGen_SWD392.Repositories.Interface;
using PhyGen_SWD392.Repositories.Repo;
using PhyGen_SWD392.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
                RoleEnum = RoleEnum.User,
                AccountTypeEnum = AccountTypeEnum.Free,
                StatusEnum = AccountStatusEnum.Active,
            };

            await _authRepo.AddAsync(newAccount);
            return true;
        }


        public async Task<Account?> LoginAsync(AccountLoginDto dto)
        {
            Account? account;

            // Phân biệt email hay username
            if (dto.Identifier.Contains("@"))
            {
                account = await _authRepo.GetAccountByEmailAsync(dto.Identifier);
                if (account == null)
                    return null;
            }
            else
            {
                account = await _authRepo.GetAccountByUsernameAsync(dto.Identifier);
                if (account == null)
                    return null;
            }
            if (account.StatusEnum != AccountStatusEnum.Active)
                return null;
            var isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, account.Password);
            return isValidPassword ? account : null;
        }

        public string GenerateJwtToken(Account acc)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, acc.Id.ToString()),
        new Claim("id", acc.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, acc.Email),
        new Claim(ClaimTypes.Role, acc.Role ?? "User"),
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
        public int GetTokenExpirationSeconds()
        {
            return _jwtSettings.ExpirationMinutes * 60;
        }

    }
}