using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;
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

        public async Task<PagedResult<MatrixDetail>> GetAllWithQueryAsync(MatrixDetailQueryParameters queryParams)
        {
            var query = _context.MatrixDetails
                .Include(t => t.Topic)
                .Include(t => t.MatrixSection)
                .AsQueryable();

            // Filter
            if (queryParams.MatrixSectionId.HasValue)
                query = query.Where(x => x.MatrixSectionId == queryParams.MatrixSectionId.Value);
            if (queryParams.TopicId.HasValue)
                query = query.Where(x => x.TopicId == queryParams.TopicId.Value);
            if (!string.IsNullOrEmpty(queryParams.DifficultyLevel))
                query = query.Where(x => x.DifficultyLevel == queryParams.DifficultyLevel);
            if (queryParams.Quantity.HasValue)
                query = query.Where(x => x.Quantity == queryParams.Quantity.Value);
            if (!string.IsNullOrEmpty(queryParams.Type))
                query = query.Where(x => x.Type == queryParams.Type);

            // Search
            if (!string.IsNullOrEmpty(queryParams.Search))
            {
                var keyword = queryParams.Search.ToLower();
                query = query.Where(x =>
                    (x.Topic != null && x.Topic.Name.ToLower().Contains(keyword)) ||
                    (x.MatrixSection != null && x.MatrixSection.SectionName.ToLower().Contains(keyword))
                );
            }

            // Sort
            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                switch (queryParams.SortBy.ToLower())
                {
                    case "difficultylevel":
                        query = queryParams.Desc ? query.OrderByDescending(x => x.DifficultyLevel) : query.OrderBy(x => x.DifficultyLevel);
                        break;
                    case "quantity":
                        query = queryParams.Desc ? query.OrderByDescending(x => x.Quantity) : query.OrderBy(x => x.Quantity);
                        break;
                    case "type":
                        query = queryParams.Desc ? query.OrderByDescending(x => x.Type) : query.OrderBy(x => x.Type);
                        break;
                    case "topicid":
                        query = queryParams.Desc ? query.OrderByDescending(x => x.TopicId) : query.OrderBy(x => x.TopicId);
                        break;
                    case "matrixsectionid":
                        query = queryParams.Desc ? query.OrderByDescending(x => x.MatrixSectionId) : query.OrderBy(x => x.MatrixSectionId);
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

            return new PagedResult<MatrixDetail>
            {
                Total = total,
                Page = queryParams.Page,
                PageSize = queryParams.PageSize,
                Items = items
            };
        }
    }
}
