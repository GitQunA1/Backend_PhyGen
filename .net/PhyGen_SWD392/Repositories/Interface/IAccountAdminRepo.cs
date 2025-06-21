using PhyGen_SWD392.Models.Entities;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IAccountAdminRepo
    {
        Task<Account> CreateAccountAsync(Account account);
        Task<IEnumerable<Account>> GetAllUserAccountsAsync();
        Task<bool> UpdateAccountAsync(Account account);
        Task<bool> DeleteAccountAsync(int id);
        Task<Account?> GetAccountByIdAsync(int id);
    }
}
