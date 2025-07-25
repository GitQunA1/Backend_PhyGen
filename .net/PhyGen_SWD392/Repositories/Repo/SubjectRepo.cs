using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Models.DTOs;
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

        public async Task<PagedResult<Subject>> GetAllWithQueryAsync(SubjectQueryParameters queryParams)
        {
            var query = _context.Subjects.AsQueryable();

            // Filter
            if (!string.IsNullOrEmpty(queryParams.Name))
                query = query.Where(x => x.Name.Contains(queryParams.Name));
            if (!string.IsNullOrEmpty(queryParams.Description))
                query = query.Where(x => x.Description.Contains(queryParams.Description));
            if (!string.IsNullOrEmpty(queryParams.Grade))
                query = query.Where(x => x.Grade == queryParams.Grade);
            if (queryParams.IsDeleted.HasValue)
                query = query.Where(x => x.IsDeleted == queryParams.IsDeleted.Value);

            // Search
            if (!string.IsNullOrEmpty(queryParams.Search))
            {
                var keyword = queryParams.Search.ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(keyword) ||
                    (x.Description != null && x.Description.ToLower().Contains(keyword)) ||
                    (x.Grade != null && x.Grade.ToLower().Contains(keyword))
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
                    case "grade":
                        query = queryParams.Desc ? query.OrderByDescending(x => x.Grade) : query.OrderBy(x => x.Grade);
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

            return new PagedResult<Subject>
            {
                Total = total,
                Page = queryParams.Page,
                PageSize = queryParams.PageSize,
                Items = items
            };
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
