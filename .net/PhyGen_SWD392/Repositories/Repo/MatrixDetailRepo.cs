using Microsoft.EntityFrameworkCore;
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

        public override async Task<MatrixDetail?> GetByIdAsync(int id)
        {
            return await _context.MatrixDetails
                .Include(t => t.Topic)
                .Include(t => t.MatrixSection)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public override async Task<List<MatrixDetail>> GetAllAsync()
        {
            return await _context.MatrixDetails
                .Include(t => t.Topic)
                .Include(t => t.MatrixSection)
                .ToListAsync();
        }
    }
}
