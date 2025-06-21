using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class MatrixSectionRepo : BaseRepo<MatrixSection, int>, IMatrixSectionRepo
    {
        private readonly PhyGenDbContext _context;
        public MatrixSectionRepo(PhyGenDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
