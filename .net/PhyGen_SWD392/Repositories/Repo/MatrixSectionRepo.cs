using Microsoft.EntityFrameworkCore;
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

        public async Task<List<MatrixSection>> GetActiveMatrixSectionsAsync()
        {
            return await _context.MatrixSections
                .Where(t => t.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<bool> SoftDelete(int id)
        {
            var section = _context.MatrixSections.Find(id);
            if (section == null)
                return false;

            section.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
