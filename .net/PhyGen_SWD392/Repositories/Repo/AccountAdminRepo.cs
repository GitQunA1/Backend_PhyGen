using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Enums;
using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Repositories.Interface;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Requests;

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

        public async Task<PagedResult<Account>> GetAllUserAccountsAsync(AccountQueryParameters queryParams)
        {
            var query = _context.Accounts
                .Where(a => a.Role != RoleEnum.admin.ToString())
                .AsQueryable();

            // Filter
            if (!string.IsNullOrEmpty(queryParams.Role))
                query = query.Where(a => a.Role == queryParams.Role);
            if (!string.IsNullOrEmpty(queryParams.Status))
                query = query.Where(a => a.Status == queryParams.Status);

            // Search
            if (!string.IsNullOrEmpty(queryParams.Search))
            {
                var keyword = queryParams.Search.ToLower();
                query = query.Where(a =>
                    a.Username.ToLower().Contains(keyword) ||
                    a.Email.ToLower().Contains(keyword) ||
                    a.School.ToLower().Contains(keyword)
                );
            }

            // Sort
            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                switch (queryParams.SortBy.ToLower())
                {
                    case "username":
                        query = queryParams.Desc ? query.OrderByDescending(a => a.Username) : query.OrderBy(a => a.Username);
                        break;
                    case "email":
                        query = queryParams.Desc ? query.OrderByDescending(a => a.Email) : query.OrderBy(a => a.Email);
                        break;
                    case "status":
                        query = queryParams.Desc ? query.OrderByDescending(a => a.Status) : query.OrderBy(a => a.Status);
                        break;
                    default:
                        query = query.OrderBy(a => a.Id); // fallback
                        break;
                }
            }
            else
            {
                query = query.OrderBy(a => a.Id);
            }

            var total = await query.CountAsync();
            var items = await query.Skip((queryParams.Page - 1) * queryParams.PageSize)
                                   .Take(queryParams.PageSize)
                                   .ToListAsync();

            return new PagedResult<Account>
            {
                Total = total,
                Page = queryParams.Page,
                PageSize = queryParams.PageSize,
                Items = items
            };
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

