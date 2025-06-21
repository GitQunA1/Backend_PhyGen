using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Repositories.Interface;
using BCrypt.Net;
using PhyGen_SWD392.Models.DTOs;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class AccountUserRepo : IAccountUser
    {
        private readonly PhyGenDbContext _context;

        public AccountUserRepo(PhyGenDbContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _context.Accounts.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> UpdateProfileAsync(int id, UserUpdateDto dto)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return false;

            account.Username = dto.Username;
            account.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                account.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            _context.Accounts.Update(account);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}