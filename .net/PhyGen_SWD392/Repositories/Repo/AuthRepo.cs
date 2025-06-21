using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class AuthRepo : IAuthRepo
    {
        private readonly PhyGenDbContext _context;

        public AuthRepo(PhyGenDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task<Account?> GetAccountByUsernameAsync(string username)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Username == username);
        }
        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Account?> GetAccountByCredentialsAsync(string email, string password)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }

        public async Task<Account?> GetAccountByUsernameAndPasswordAsync(string username, string password)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
            if (account == null) return null;

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, account.Password);
            return isPasswordValid ? account : null;
        }


    }
}