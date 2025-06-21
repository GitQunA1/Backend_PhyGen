using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Admin")]

    public class AccountUserController : ControllerBase
    {
        private readonly AccountUserService _accountUserService;

        public AccountUserController(AccountUserService accountUserService)
        {
            _accountUserService = accountUserService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _accountUserService.GetByIdAsync(id);
            if (account == null)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(
                    new List<string> { "User not found." },
                    "Failed to retrieve user."));
            }

            return Ok(ApiResponse<Account>.SuccessResponse(account, "User retrieved successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UserUpdateDto dto)
        {
            var success = await _accountUserService.UpdateProfileAsync(id, dto);
            if (!success)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(
                    new List<string> { "Update failed. User not found." },
                    "Failed to update user profile."));
            }

            return Ok(ApiResponse<string>.SuccessResponse(null, "User profile updated successfully."));
        }
    }
}