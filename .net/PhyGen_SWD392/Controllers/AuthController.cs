using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Services;
using PhyGen_SWD392.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PhyGen_SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly EmailVerificationService _service;
        private readonly GoogleAuthService _googleAuthService;

        public AuthController(AuthService authService, EmailVerificationService service, GoogleAuthService googleAuthService)
        {
            _authService = authService;
            _service = service;
            _googleAuthService = googleAuthService;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result)
            {
                var error = new List<string> { "Email already exists or passwords do not match." };
                return BadRequest(ApiResponse<string>.ErrorResponse(error, "Register failed"));
            }
            var verifySent = await _service.RequestEmailVerificationAsync(dto.Email);
            if (!verifySent)
            {
                return Ok(ApiResponse<string>.SuccessResponse(
                    null, "Register successful, but failed to send verification code."));
            }

            return Ok(ApiResponse<string>.SuccessResponse(
                null, "Register successful. Verification code sent to email."));
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginDto dto)
        {
            var account = await _authService.LoginAsync(dto);

            if (account == null)
            {
                var error = new List<string> { "Invalid email/password or inactive account." };
                return Unauthorized(ApiResponse<string>.ErrorResponse(error, "Login failed"));
            }
            var token = _authService.GenerateJwtToken(account);
            var expiresIn = _authService.GetTokenExpirationMinutes();

            var response = new
            {
                Token = token,
                ExpiresIn = expiresIn,
                Account = new
                {
                    account.Username,
                    account.Email,
                    account.EmailVerified,
                    account.AccountType,
                    Role = account.RoleEnum.ToString()
                }
            };

            return Ok(ApiResponse<object>.SuccessResponse(response, "Login successful"));
        }

        [AllowAnonymous]
        [HttpPost("login/google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] FirebaseLoginDto dto)
        {
            var (token, account) = await _googleAuthService.LoginWithGoogleAsync(dto.IdToken);

            if (token == null) return Unauthorized("Invalid Google token");

            return Ok(new
            {
                token,
                expiresIn = 3600,
                user = new
                {
                    account.Id,
                    account.Email,
                    account.Username,
                    account.RoleEnum
                }
            });
        }
    }
}