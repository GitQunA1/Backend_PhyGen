using PhyGen_SWD392.Models.Entities;
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

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _accountRepo.GetAllUserAccountsAsync();
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _accountRepo.GetAccountByIdAsync(id);
        }

        public async Task<bool> UpdateAccountAsync(Account account)
        {
            return await _accountRepo.UpdateAccountAsync(account);
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            return await _accountRepo.DeleteAccountAsync(id);
        }
    }
}