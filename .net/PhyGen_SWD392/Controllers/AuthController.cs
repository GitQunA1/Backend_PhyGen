using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Dtos;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Services;
using PhyGen_SWD392.Settings;

namespace PhyGen_SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
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

            return Ok(ApiResponse<string>.SuccessResponse(null, "Register successful"));
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
            var expiresIn = _authService.GetTokenExpirationSeconds(); // gợi ý bên dưới

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



        [Authorize]
        [HttpGet("secure-endpoint")]
        public IActionResult SecureEndpoint()
        {
            return Ok("You're authenticated!");
        }
    }
}