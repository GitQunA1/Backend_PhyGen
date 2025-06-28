using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
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
            var existingAccount = await _accountService.GetAccountByIdAsync(id);
            if (existingAccount == null)
            {
                var error = ApiResponse<string>.ErrorResponse(
                    new() { "Account not found." },
                    "Update failed."
                );
                return NotFound(error);
            }


            existingAccount.Username = !string.IsNullOrWhiteSpace(dto.Username) ? dto.Username : existingAccount.Username;
            existingAccount.Password = !string.IsNullOrWhiteSpace(dto.Password) ? dto.Password : existingAccount.Password;
            existingAccount.Email = !string.IsNullOrWhiteSpace(dto.Email) ? dto.Email : existingAccount.Email;
            existingAccount.EmailVerified = dto.EmailVerified ?? existingAccount.EmailVerified;

            if (Enum.IsDefined(typeof(RoleEnum), dto.Role))
                existingAccount.Role = Enum.GetName(typeof(RoleEnum), dto.Role)!;

            if (Enum.IsDefined(typeof(AccountTypeEnum), dto.AccountType))
                existingAccount.AccountType = Enum.GetName(typeof(AccountTypeEnum), dto.AccountType)!;

            if (Enum.IsDefined(typeof(AccountStatusEnum), dto.AccountStatus))
                existingAccount.Status = Enum.GetName(typeof(AccountStatusEnum), dto.AccountStatus)!;

            var result = await _accountService.UpdateAccountAsync(existingAccount);

            if (!result)
            {
                var error = ApiResponse<string>.ErrorResponse(
                    new() { "Failed to update account." },
                    "Update failed."
                );
                return BadRequest(error);
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