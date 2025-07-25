using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class QuestionRepo : BaseRepo<Question, int>, IQuestionRepo
    {
        private readonly PhyGenDbContext _context;
        public QuestionRepo(PhyGenDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Question?> GetByIdAsync(int id)
        {
            return await _context.Questions
                .Include(q => q.Answers.Where(a => a.IsDeleted != true))
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<Question>> GetQuestionsForMatrixDetailAsync(List<MatrixDetail> details)
        {
            var topicIds = details.Select(d => d.TopicId).Distinct().ToList();
            var levels = details.Select(d => d.DifficultyLevel).Distinct().ToList();

            return await _context.Questions
                .Include(q => q.Topic)
                .Where(q =>
                    topicIds.Contains(q.TopicId) &&
                    q.Status == "active")
                .ToListAsync();
        }
    }
}
