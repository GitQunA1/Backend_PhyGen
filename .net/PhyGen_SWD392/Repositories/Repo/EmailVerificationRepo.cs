using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Repositories.Interface;
using System.Collections.Concurrent;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class EmailVerificationRepo : IEmailVerificationRepo
    {
        private readonly PhyGenDbContext _context;
        private readonly IEmailService _emailService;

        // Lưu mã xác thực tạm
        private static readonly ConcurrentDictionary<string, EmailVerifyInfo> _verifyStore = new();

        public EmailVerificationRepo(PhyGenDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<bool> RequestEmailVerificationAsync(string email)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            if (account == null) return false;

            var code = Guid.NewGuid().ToString("N")[..6].ToUpper();

            _verifyStore[email] = new EmailVerifyInfo
            {
                Code = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)
            };

            await _emailService.SendAsync(email, "Email Verification Code", $"Your verification code is: {code}");
            return true;
        }

        public async Task<bool> VerifyEmailAsync(string email, string code)
        {
            if (!_verifyStore.TryGetValue(email, out var info))
                return false;

            if (info.Code != code || info.ExpiresAt < DateTime.UtcNow)
                return false;

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            if (account == null) return false;

            account.EmailVerified = true;
            _verifyStore.TryRemove(email, out _);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
