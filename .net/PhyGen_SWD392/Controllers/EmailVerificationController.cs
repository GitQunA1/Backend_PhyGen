using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Dtos;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailVerificationController : ControllerBase
    {
        private readonly EmailVerificationService _service;

        public EmailVerificationController(EmailVerificationService service)
        {
            _service = service;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestVerification([FromBody] string email)
        {
            var success = await _service.RequestEmailVerificationAsync(email);
            if (!success)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(
                    new List<string> { "Email not found." }, "Unable to send verification code"));
            }

            return Ok(ApiResponse<string>.SuccessResponse(null, "Verification code sent to email"));
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto dto)
        {
            var success = await _service.VerifyEmailAsync(dto.Email, dto.Code);

            if (!success)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(
                    new List<string> { "Invalid or expired code." }, "Email verification failed"));
            }

            return Ok(ApiResponse<string>.SuccessResponse(null, "Email verified successfully"));
        }
    }
}