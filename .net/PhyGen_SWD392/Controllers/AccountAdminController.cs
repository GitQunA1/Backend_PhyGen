using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Dtos;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Enums;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AccountAdminController : ControllerBase
    {
        private readonly AccountAdminService _accountService;

        public AccountAdminController(AccountAdminService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountCreateDto dto)
        {
            var account = new Account
            {
                Username = dto.Username,
                Password = dto.Password,
                Email = dto.Email,
                EmailVerified = dto.EmailVerified,
                Role = dto.Role.ToString(),  // "Admin", "User"
                AccountType = dto.AccountType.ToString(),  // "Free", "Premium"
                StatusEnum = AccountStatusEnum.Active
            };

            var created = await _accountService.CreateAccountAsync(account);
            var response = ApiResponse<Account>.SuccessResponse(created, "Account created successfully.");
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccount()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            var response = ApiResponse<IEnumerable<Account>>.SuccessResponse(accounts, "List all accounts:");
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                var error = ApiResponse<string>.ErrorResponse(new() { "Account not found." }, "Not found.");
                return NotFound(error);
            }

            var response = ApiResponse<Account>.SuccessResponse(account, "Account retrieved successfully.");
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountUpdateDto dto)
        {
            var account = new Account
            {
                Id = id,
                Username = dto.Username,
                Password = dto.Password,
                Email = dto.Email,
                EmailVerified = dto.EmailVerified ?? false,
                Role = dto.Role.ToString(),
                AccountType = dto.AccountType.ToString(),
                Status = dto.AccountStatus.ToString()
            };

            var result = await _accountService.UpdateAccountAsync(account);

            if (!result)
            {
                var error = ApiResponse<string>.ErrorResponse(
                    new() { "Account not found for update." },
                    "Update failed."
                );
                return NotFound(error);
            }

            var response = ApiResponse<string>.SuccessResponse(
                "Account updated successfully.",
                "Success"
            );
            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _accountService.DeleteAccountAsync(id);
            if (!result)
            {
                var error = ApiResponse<string>.ErrorResponse(new() { "Account not found for deletion." }, "Delete failed.");
                return NotFound(error);
            }

            var response = ApiResponse<string>.SuccessResponse("Account status set to Inactive.", "Account deleted successfully.");
            return Ok(response);
        }
    }
}