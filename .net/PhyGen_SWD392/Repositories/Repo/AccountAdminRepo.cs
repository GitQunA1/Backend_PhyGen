using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Enums;
using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class AccountAdminRepo : IAccountAdminRepo
    {
        private readonly PhyGenDbContext _context;

        public AccountAdminRepo(PhyGenDbContext context)
        {
            _context = context;
        }
        public async Task<Account> CreateAccountAsync(Account account)
        {
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            account.StatusEnum = AccountStatusEnum.active;
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<IEnumerable<Account>> GetAllUserAccountsAsync()
        {
            return await _context.Accounts
                .Where(a => a.Role != RoleEnum.admin.ToString())
                .ToListAsync();
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<bool> UpdateAccountAsync(Account updatedAccount)
        {
            var account = await _context.Accounts.FindAsync(updatedAccount.Id);
            if (account == null) return false;

            account.Username = updatedAccount.Username;

            //Kiểm tra mật khẩu cũ với mật khẩu mới có trùng không
            if (!BCrypt.Net.BCrypt.Verify(updatedAccount.Password, account.Password))
            {
                account.Password = BCrypt.Net.BCrypt.HashPassword(updatedAccount.Password);
            }

            account.Email = updatedAccount.Email;
            account.EmailVerified = updatedAccount.EmailVerified;
            account.Role = updatedAccount.Role;
            account.AccountType = updatedAccount.AccountType;
            account.Status = updatedAccount.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return false;

            account.StatusEnum = AccountStatusEnum.inactive;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

