using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
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
    }
}
