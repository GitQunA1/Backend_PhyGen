using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;
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

        public async Task<PagedResult<MatrixSection>> GetAllWithQueryAsync(MatrixSectionQueryParameters queryParams)
        {
            var query = _context.MatrixSections
                .Include(x => x.Matrix)
                .AsQueryable();

            // Filter
            if (queryParams.MatrixId.HasValue)
                query = query.Where(x => x.MatrixId == queryParams.MatrixId.Value);
            if (queryParams.IsDeleted.HasValue)
                query = query.Where(x => x.IsDeleted == queryParams.IsDeleted.Value);
            if (!string.IsNullOrEmpty(queryParams.SectionName))
                query = query.Where(x => x.SectionName.Contains(queryParams.SectionName));
            if (queryParams.DisplayOrder.HasValue)
                query = query.Where(x => x.DisplayOrder == queryParams.DisplayOrder.Value);

            // Search
            if (!string.IsNullOrEmpty(queryParams.Search))
            {
                var keyword = queryParams.Search.ToLower();
                query = query.Where(x =>
                    (x.SectionName != null && x.SectionName.ToLower().Contains(keyword)) ||
                    (x.Matrix != null && x.Matrix.Id.ToString().Contains(keyword))
                );
            }

            // Sort
            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                switch (queryParams.SortBy.ToLower())
                {
                    case "sectionname":
                        query = queryParams.Desc ? query.OrderByDescending(x => x.SectionName) : query.OrderBy(x => x.SectionName);
                        break;
                    case "displayorder":
                        query = queryParams.Desc ? query.OrderByDescending(x => x.DisplayOrder) : query.OrderBy(x => x.DisplayOrder);
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

            return new PagedResult<MatrixSection>
            {
                Total = total,
                Page = queryParams.Page,
                PageSize = queryParams.PageSize,
                Items = items
            };
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
