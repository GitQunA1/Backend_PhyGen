using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IAccountAdminRepo
    {
        Task<Account> CreateAccountAsync(Account account);
        Task<PagedResult<Account>> GetAllUserAccountsAsync(AccountQueryParameters queryParams);
        Task<bool> UpdateAccountInfoAsync(Account account);
        Task<bool> UpdateAccountPasswordAsync(int accountId, string newPassword);
        Task<bool> DeleteAccountAsync(int id);
        Task<Account?> GetAccountByIdAsync(int id);
    }
}
