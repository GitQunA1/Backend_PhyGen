using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Enums;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [Route("api/account_admins")]
    [ApiController]
    [Authorize(Roles = "admin")]
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
                Role = dto.Role.ToString(),
                AccountType = dto.AccountType.ToString(),
                StatusEnum = AccountStatusEnum.active
            };

            var created = await _accountService.CreateAccountAsync(account);
            var response = ApiResponse<Account>.SuccessResponse(created, "Account created successfully.");
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccount([FromQuery] AccountQueryParameters queryParams)
        {
            var result = await _accountService.GetAllAccountsAsync(queryParams);
            return Ok(ApiResponse<PagedResult<Account>>.SuccessResponse(result, "List all accounts:"));
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

        [HttpPut("{id}/info")]
        public async Task<IActionResult> UpdateInfoAccount(int id, [FromBody] AccountUpdateDto dto)
        {
            var existingAccount = await _accountService.GetAccountByIdAsync(id);
            if (existingAccount == null)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(
                    new() { "Account not found." },
                    "Update failed."
                ));
            }

            existingAccount.Username = !string.IsNullOrWhiteSpace(dto.Username) ? dto.Username : existingAccount.Username;
            existingAccount.Email = !string.IsNullOrWhiteSpace(dto.Email) ? dto.Email : existingAccount.Email;
            existingAccount.EmailVerified = dto.EmailVerified ?? existingAccount.EmailVerified;

            if (dto.Role.HasValue)
                existingAccount.Role = dto.Role.ToString();

            if (dto.AccountType.HasValue)
                existingAccount.AccountType = dto.AccountType.ToString();

            if (dto.AccountStatus.HasValue)
                existingAccount.Status = dto.AccountStatus.ToString();

            var result = await _accountService.UpdateAccountInfoAsync(existingAccount);

            if (!result)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(
                    new() { "Failed to update account info." },
                    "Update failed."
                ));
            }

            return Ok(ApiResponse<string>.SuccessResponse(
                "Account info updated successfully.",
                "Success"
            ));
        }
        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdatePassword(int id, [FromBody] AccountPasswordUpdateDto dto)
        {
            var existingAccount = await _accountService.GetAccountByIdAsync(id);
            if (existingAccount == null)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(
                    new() { "Account not found." },
                    "Update failed."
                ));
            }

            var result = await _accountService.UpdateAccountPasswordAsync(existingAccount.Id, dto.Password);

            if (!result)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(
                    new() { "Failed to update password." },
                    "Update failed."
                ));
            }

            return Ok(ApiResponse<string>.SuccessResponse(
                "Password updated successfully.",
                "Success"
            ));
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