using PhyGen_SWD392.Models.Entities;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IAuthRepo
    {
        Task<Account?> GetAccountByEmailAsync(string email);
        Task<Account?> GetAccountByCredentialsAsync(string email, string password);
        Task AddAsync(Account account); 
    }
}
