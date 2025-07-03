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
        private readonly ImageService _imageService;


        public AccountUserController(AccountUserService accountUserService, ImageService imageService)
        {
            _accountUserService = accountUserService;
            _imageService = imageService;
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

        [HttpPost("upload-avatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] FileUploadDTO dto)
        {
            var userIdStr = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized("Invalid user");

            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file uploaded");

            var imageUrl = await _imageService.UploadImageAsync(dto.File);

            await _accountUserService.UpdateAvatarUrlAsync(userId, imageUrl);

            return Ok(new { avatarUrl = imageUrl });
        }
    }
}