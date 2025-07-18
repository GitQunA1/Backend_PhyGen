using Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Enums;
using PhyGen_SWD392.Repositories.Interface;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly PhyGenDbContext _context;

        public DashboardRepository(PhyGenDbContext context)
        {
            _context = context;
        }

        public async Task<UserOverviewDto> GetUserOverviewAsync()
        {
            var now = DateTime.UtcNow;

            var totalUsers = await _context.Accounts.CountAsync();

            var verifiedUsers = await _context.Accounts.CountAsync(u => u.EmailVerified == true);

            var rawUsersByType = await _context.Accounts
                .GroupBy(u => u.AccountType)
                .Select(g => new { AccountType = g.Key, Count = g.Count() })
                .ToListAsync();

            var usersByType = rawUsersByType
               .Where(x => Enum.TryParse<AccountTypeEnum>(x.AccountType, true, out _))
               .ToDictionary(
                   x => Enum.Parse<AccountTypeEnum>(x.AccountType, true),
                   x => x.Count
               );

            return new UserOverviewDto
            {
                TotalUsers = totalUsers,
                EmailVerifiedUsers = verifiedUsers,
                UsersByAccountType = usersByType
            };
        }
    }
}
