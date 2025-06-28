using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Enums;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class ExamMatrixRepo : BaseRepo<ExamMatrix, int>, IExamMatrixRepo
    {
        private readonly PhyGenDbContext _context;
        public ExamMatrixRepo(PhyGenDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ExamMatrix>> GetActiveExamMatricesAsync()
        {
            string activeStatus = ExamMatrixStatusEnum.active.ToString();
            return await _context.ExamMatrices
                .Where(t => t.Status == activeStatus)
                .ToListAsync();
        }

        public async Task<bool> SoftDelete(int id)
        {
            var examMatrix = _context.ExamMatrices.Find(id);
            if (examMatrix == null)
                return false;

            examMatrix.Status = ExamMatrixStatusEnum.inactive.ToString();
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
