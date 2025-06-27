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
        public async Task<List<ExamListDto>> GetExamsByUserAsync(int userId)
        {
            var examList = await (from e in _context.Exams
                                  join em in _context.ExamMatrices on e.Id equals em.Id
                                  join s in _context.Subjects on em.SubjectId equals s.Id
                                  where e.CreatedBy == userId
                                  select new ExamListDto
                                  {
                                      Id = e.Id,
                                      IsDraft = e.IsDraft,
                                      ExamType = e.Examtype,
                                      CreatedAt = e.CreatedAt,
                                      QuestionCount = _context.ExamQuestions
                                          .Count(eq => eq.Section != null && eq.Section.ExamId == e.Id),
                                      SubjectName = s.Name
                                  }).ToListAsync();

            return examList;
        }
    }
}