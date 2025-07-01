using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Repositories.Interface;
using PhyGen_SWD392.Services;
using System.Security.Claims;

namespace PhyGen_SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "user,admin")]

    public class AccountUserController : ControllerBase
    {
        private readonly AccountUserService _accountUserService;
        private readonly SupabaseStorageService _supabaseStorageService;


        public AccountUserController(AccountUserService accountUserService, SupabaseStorageService supabaseStorageService)
        {
            _accountUserService = accountUserService;
            _supabaseStorageService = supabaseStorageService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("Invalid user ID.");

            var account = await _accountUserService.GetByIdAsync(userId);
            if (account == null)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(
                    new List<string> { "User not found." },
                    "Failed to retrieve user."));
            }

            return Ok(ApiResponse<Account>.SuccessResponse(account, "User retrieved successfully."));
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UserUpdateDto dto)
        {
            var userIdClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("Invalid user ID.");

            var success = await _accountUserService.UpdateProfileAsync(userId, dto);
            if (!success)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(
                    new List<string> { "Update failed. User not found." },
                    "Failed to update user profile."));
            }

            return Ok(ApiResponse<string>.SuccessResponse(null, "User profile updated successfully."));
        }


        [HttpGet("me/exams")]
        public async Task<IActionResult> GetMyExams()
        {
            var userIdClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("Invalid user ID.");

            var exams = await _accountUserService.GetExamsByUserAsync(userId);

            return Ok(ApiResponse<List<ExamListDto>>.SuccessResponse(exams, "Exam List retrieved successfully."));
        }

        [HttpGet("me/avatar")]
        public async Task<IActionResult> GetAvatarUrl()
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var account = await _accountUserService.GetByIdAsync(int.Parse(userId));
            if (account == null || string.IsNullOrEmpty(account.AvatarUrl))
                return NotFound("No avatar");

            var signedUrl = await _supabaseStorageService.GenerateSignedAvatarUrl(account.AvatarUrl);

            return Ok(new { url = signedUrl });
        }


        [HttpPost("me/avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadUserImage([FromForm] AvatarUploadRequest request)
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { error = "User ID not found in token." });

            var file = request.File;

            if (file == null || file.Length == 0)
                return BadRequest(new { error = "No file provided." });

            try
            {
                var result = await _accountUserService.UploadAvatarAsyncSupabase(file, userId);

                return Ok(new
                {
                    path = result.Path,
                    signedUrl = result.SignedUrl
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}