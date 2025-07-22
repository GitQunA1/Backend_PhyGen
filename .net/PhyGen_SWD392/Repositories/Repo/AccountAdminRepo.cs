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

            if (!string.IsNullOrEmpty(queryParams.Role))
                query = query.Where(a => a.Role == queryParams.Role);

            if (!string.IsNullOrEmpty(queryParams.Status))
                query = query.Where(a => a.Status == queryParams.Status);

            if (!string.IsNullOrEmpty(queryParams.AccountType))
            {
                if (Enum.TryParse<AccountTypeEnum>(queryParams.AccountType, true, out var accountTypeEnum))
                {
                    query = query.Where(a => a.AccountType == accountTypeEnum.ToString());
                }
            }

            if (queryParams.EmailVerified.HasValue)
            {
                query = query.Where(a => a.EmailVerified == queryParams.EmailVerified.Value);
            }

            if (!string.IsNullOrEmpty(queryParams.Search))
            {
                var keyword = queryParams.Search.ToLower();
                query = query.Where(a =>
                    a.Username.ToLower().Contains(keyword) ||
                    a.Email.ToLower().Contains(keyword) ||
                    (a.School != null && a.School.ToLower().Contains(keyword)) ||
                    (a.Address != null && a.Address.ToLower().Contains(keyword)) ||
                    (a.Status != null && a.Status.ToLower().Contains(keyword)) ||
                    (a.AccountType != null && a.AccountType.ToLower().Contains(keyword)) ||
                    (a.Role != null && a.Role.ToLower().Contains(keyword))
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
                    case "accounttype":
                        query = queryParams.Desc ? query.OrderByDescending(a => a.AccountType) : query.OrderBy(a => a.AccountType);
                        break;
                    case "emailverified":
                        query = queryParams.Desc ? query.OrderByDescending(a => a.EmailVerified) : query.OrderBy(a => a.EmailVerified);
                        break;
                    case "school":
                        query = queryParams.Desc ? query.OrderByDescending(a => a.School) : query.OrderBy(a => a.School);
                        break;
                    case "address":
                        query = queryParams.Desc ? query.OrderByDescending(a => a.Address) : query.OrderBy(a => a.Address);
                        break;
                    case "role":
                        query = queryParams.Desc ? query.OrderByDescending(a => a.Role) : query.OrderBy(a => a.Role);
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

            account.Username = updatedAccount.Username ?? account.Username;
            account.Email = updatedAccount.Email ?? account.Email;
            account.EmailVerified = updatedAccount.EmailVerified;
            account.Role = updatedAccount.Role;
            account.AccountType = updatedAccount.AccountType;
            account.Status = updatedAccount.Status;

            if (!string.IsNullOrWhiteSpace(updatedAccount.Password))
            {
                try
                {
                    bool isSame = false;

                    if (!string.IsNullOrWhiteSpace(account.Password))
                    {
                        isSame = BCrypt.Net.BCrypt.Verify(updatedAccount.Password, account.Password);
                    }

                    if (!isSame)
                    {
                        account.Password = BCrypt.Net.BCrypt.HashPassword(updatedAccount.Password);
                    }
                }
                catch (BCrypt.Net.SaltParseException)
                {
                    account.Password = BCrypt.Net.BCrypt.HashPassword(updatedAccount.Password);
                }
            }

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

