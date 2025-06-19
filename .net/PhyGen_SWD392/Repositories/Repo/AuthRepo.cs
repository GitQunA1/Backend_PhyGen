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

        public async Task<Account?> GetAccountByCredentialsAsync(string email, string password)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email == email);
        }
    }
}