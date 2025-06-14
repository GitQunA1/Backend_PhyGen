using PhyGen_SWD392.Models;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class TopicRepo : BaseRepo<Topic, int> , ITopicRepo
    {
        private readonly PhyGenDbContext _context;
        public TopicRepo(PhyGenDbContext phyGenDbContext) : base(phyGenDbContext)
        {
            _context = phyGenDbContext;
        }
    }

}
