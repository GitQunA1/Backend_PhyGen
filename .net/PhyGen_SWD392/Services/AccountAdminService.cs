using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Services
{
    public class AccountAdminService
    {
        private readonly IAccountAdminRepo _accountRepo;

        public AccountAdminService(IAccountAdminRepo accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            return await _accountRepo.CreateAccountAsync(account);
        }

        public async Task<PagedResult<Account>> GetAllAccountsAsync(AccountQueryParameters queryParams)
        {
            return await _accountRepo.GetAllUserAccountsAsync(queryParams);
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _accountRepo.GetAccountByIdAsync(id);
        }

        public async Task<bool> UpdateAccountInfoAsync(Account account)
        {
            return await _accountRepo.UpdateAccountInfoAsync(account);
        }

        public async Task<bool> UpdateAccountPasswordAsync(int accountId, string newPassword)
        {
            return await _accountRepo.UpdateAccountPasswordAsync(accountId, newPassword);
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            return await _accountRepo.DeleteAccountAsync(id);
        }
    }
}
