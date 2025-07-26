using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class ExamVersionRepo : BaseRepo<ExamVersion, int>, IExamVersionRepo
    {
        private readonly PhyGenDbContext _context;
        public ExamVersionRepo(PhyGenDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ExamVersion>> GetAllWithQueryAsync(ExamVersionQueryParameters parameters)
        {
            var query = _context.ExamVersions
            .Include(e => e.FinalExam)
            .AsQueryable();

            // Apply filters
            if (parameters.FinalExamId.HasValue)
            {
                query = query.Where(x => x.FinalExamId == parameters.FinalExamId);
            }

            if (!string.IsNullOrEmpty(parameters.VersionCode))
            {
                query = query.Where(x => x.VersionCode.Contains(parameters.VersionCode));
            }

            return await query.ToListAsync();
        }
    }
}
