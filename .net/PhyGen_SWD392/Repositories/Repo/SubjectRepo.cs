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
    }
}
