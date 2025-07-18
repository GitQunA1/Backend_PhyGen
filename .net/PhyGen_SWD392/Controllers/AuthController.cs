using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Services;

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
            var message = verifySent
                ? "Register successful. Verification code sent to email."
                : "Register successful, but failed to send verification code.";

            return Ok(ApiResponse<string>.SuccessResponse(null, message));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginDto dto)
        {
            var result = await _authService.LoginWithTokenAsync(dto);
            if (result == null)
                return Unauthorized(ApiResponse<string>.ErrorResponse(
                    new List<string> { "Invalid email/password or inactive account." }, "Login failed"));

            var account = await _authService.LoginAsync(dto); 

            var response = new
            {
                token = result.AccessToken,
                refreshToken = result.RefreshToken,
                expiresIn = result.ExpiresIn,
                user = new
                {
                    account!.Id,
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

            if (token == null)
            {
                return Unauthorized(ApiResponse<string>.ErrorResponse(
                    new List<string> { "Invalid Google token" },
                    "Login with Google failed"
                ));
            }

            var response = new
            {
                token,
                expiresIn = 3600,
                user = new
                {
                    account.Id,
                    account.Email,
                    account.Username,
                    Role = account.RoleEnum.ToString()
                }
            };

            return Ok(ApiResponse<object>.SuccessResponse(response, "Login with Google successful"));
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] string refreshToken)
        {
            var result = _authService.RefreshAccessToken(refreshToken);
            if (result == null)
                return Unauthorized(ApiResponse<string>.ErrorResponse(
                    new[] { "Invalid or expired refresh token" }.ToList(),
                    "Refresh token failed"
                ));

            return Ok(ApiResponse<AuthResultDto>.SuccessResponse(result, "Token refreshed successfully"));
        }
    }
}
