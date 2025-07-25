using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Models.DTOs;
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

        public async Task<PagedResult<Topic>> GetAllWithQueryAsync(TopicQueryParameters queryParams)
        {
            var query = _context.Topics
                .Include(t => t.Subject)
                .AsQueryable();

            // Filter
            if (!string.IsNullOrEmpty(queryParams.Name))
                query = query.Where(x => x.Name.Contains(queryParams.Name));
            if (!string.IsNullOrEmpty(queryParams.Level))
                query = query.Where(x => x.Level == queryParams.Level);
            if (queryParams.SubjectId.HasValue)
                query = query.Where(x => x.SubjectId == queryParams.SubjectId.Value);
            if (queryParams.ParentId.HasValue)
                query = query.Where(x => x.ParentId == queryParams.ParentId.Value);
            if (queryParams.IsDeleted.HasValue)
                query = query.Where(x => x.IsDeleted == queryParams.IsDeleted.Value);

            // Search
            if (!string.IsNullOrEmpty(queryParams.Search))
            {
                var keyword = queryParams.Search.ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(keyword) ||
                    (x.Subject != null && x.Subject.Name.ToLower().Contains(keyword))
                );
            }

            // Sort
            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                switch (queryParams.SortBy.ToLower())
                {
                    case "name":
                        query = queryParams.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                        break;
                    case "level":
                        query = queryParams.Desc ? query.OrderByDescending(x => x.Level) : query.OrderBy(x => x.Level);
                        break;
                    case "subjectid":
                        query = queryParams.Desc ? query.OrderByDescending(x => x.SubjectId) : query.OrderBy(x => x.SubjectId);
                        break;
                    default:
                        query = query.OrderBy(x => x.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(x => x.Id);
            }

            // Paging
            var total = await query.CountAsync();
            var items = await query.Skip((queryParams.Page - 1) * queryParams.PageSize)
                                   .Take(queryParams.PageSize)
                                   .ToListAsync();

            return new PagedResult<Topic>
            {
                Total = total,
                Page = queryParams.Page,
                PageSize = queryParams.PageSize,
                Items = items
            };
        }
        
    }
}
