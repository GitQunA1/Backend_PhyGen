using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class SubjectRepo : BaseRepo<Subject, int>, ISubjectRepo
    {
        private readonly PhyGenDbContext _context;
        public SubjectRepo(PhyGenDbContext phyGenDbContext) : base(phyGenDbContext)
        {
            _context = phyGenDbContext;
        }

        public async Task<List<Subject>> GetActiveSubjectsAsync()
        {
            return await _context.Subjects
                .Where(t => t.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<bool> SoftDelete(int id)
        {
            var subject = _context.Subjects.Find(id);
            if (subject == null)
                return false;

            subject.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
