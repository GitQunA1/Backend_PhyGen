using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class TopicRepo : BaseRepo<Topic, int>, ITopicRepo
    {
        private readonly PhyGenDbContext _context;
        public TopicRepo(PhyGenDbContext phyGenDbContext) : base(phyGenDbContext)
        {
            _context = phyGenDbContext;
        }

        public override async Task<Topic?> GetByIdAsync(int id)
        {
            return await _context.Topics
                .Include(t => t.Subject)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public override async Task<List<Topic>> GetAllAsync()
        {
            return await _context.Topics
                .Include(t => t.Subject)
                .ToListAsync();
        }

        public async Task<List<Topic>> GetActiveTopicsAsync()
        {
            return await _context.Topics
                .Include(t => t.Subject)
                .Where(t => t.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<bool> SoftDelete(int id)
        {
            var topic = _context.Topics.Find(id);
            if (topic == null)
                return false;

            topic.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
