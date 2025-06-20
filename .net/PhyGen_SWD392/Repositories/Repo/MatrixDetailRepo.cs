using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class MatrixDetailRepo : BaseRepo<MatrixDetail, int>, IMatrixDetailRepo
    {
        private readonly PhyGenDbContext _context;
        public MatrixDetailRepo(PhyGenDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
