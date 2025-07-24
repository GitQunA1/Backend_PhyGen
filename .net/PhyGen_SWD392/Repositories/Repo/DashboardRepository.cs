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

            var normalAccounts = _context.Accounts
                .Where(u => u.Role.ToLower() != "manager" && u.Role.ToLower() != "admin");

            var totalUsers = await normalAccounts.CountAsync();

            var verifiedUsers = await normalAccounts
                .CountAsync(u => u.EmailVerified == true);

            var rawUsersByType = await normalAccounts
                .GroupBy(u => u.AccountType)
                .Select(g => new { AccountType = g.Key, Count = g.Count() })
                .ToListAsync();

            var usersByType = rawUsersByType
               .Where(x => Enum.TryParse<AccountTypeEnum>(x.AccountType, true, out _))
               .ToDictionary(
                   x => Enum.Parse<AccountTypeEnum>(x.AccountType, true),
                   x => x.Count
               );

            var adminCount = await _context.Accounts
                .CountAsync(u => u.Role.ToLower() == "admin");

            var managerCount = await _context.Accounts
                .CountAsync(u => u.Role.ToLower() == "manager");

            return new UserOverviewDto
            {
                TotalUsers = totalUsers,
                EmailVerifiedUsers = verifiedUsers,
                UsersByAccountType = usersByType,
                AdminCount = adminCount,
                ManagerCount = managerCount
            };
        }

    }
}
