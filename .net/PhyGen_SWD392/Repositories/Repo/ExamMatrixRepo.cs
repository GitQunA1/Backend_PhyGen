using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Enums;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class ExamMatrixRepo : BaseRepo<ExamMatrix, int>, IExamMatrixRepo
    {
        private readonly PhyGenDbContext _context;
        public ExamMatrixRepo(PhyGenDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ExamMatrix>> GetActiveExamMatricesAsync()
        {
            string activeStatus = ExamMatrixStatusEnum.active.ToString();
            return await _context.ExamMatrices
                .Where(t => t.Status == activeStatus)
                .ToListAsync();
        }

        public async Task<bool> SoftDelete(int id)
        {
            var examMatrix = _context.ExamMatrices.Find(id);
            if (examMatrix == null)
                return false;

            examMatrix.Status = ExamMatrixStatusEnum.inactive.ToString();
            await _context.SaveChangesAsync();
            return true;
        }

        public override async Task<ExamMatrix?> GetByIdAsync(int id)
        {
            return await _context.ExamMatrices
                .Include(e => e.Subject)
                .Include(e => e.MatrixSections)
                .ThenInclude(ms => ms.MatrixDetails)
                .FirstOrDefaultAsync(e => e.Id == id && e.Status == ExamMatrixStatusEnum.active.ToString());
        }

        public async Task<PagedResult<ExamMatrix>> GetAllWithQueryAsync(ExamMatrixQueryParameters queryParams)
        {
            var query = _context.ExamMatrices
                .Include(e => e.Subject)
                .Include(e => e.MatrixSections)
                .ThenInclude(ms => ms.MatrixDetails)
                .AsQueryable();

            // Filter
            if (queryParams.SubjectId.HasValue)
                query = query.Where(x => x.SubjectId == queryParams.SubjectId.Value);
            if (!string.IsNullOrEmpty(queryParams.Status))
                query = query.Where(x => x.Status == queryParams.Status);
            if (!string.IsNullOrEmpty(queryParams.ExamType))
                query = query.Where(x => x.Examtype == queryParams.ExamType);

            // Search
            if (!string.IsNullOrEmpty(queryParams.Search))
            {
                string keyword = queryParams.Search.ToLower();
                query = query.Where(x =>
                    (x.Examtype != null && x.Examtype.ToLower().Contains(keyword)) ||
                    (x.Subject != null && x.Subject.Name.ToLower().Contains(keyword))
                );
            }

            // Sort
            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                switch (queryParams.SortBy.ToLower())
                {
                    case "createdby":
                        query = queryParams.Desc ? query.OrderByDescending(x => x.CreatedBy) : query.OrderBy(x => x.CreatedBy);
                        break;
                    case "subjectid":
                        query = queryParams.Desc ? query.OrderByDescending(x => x.SubjectId) : query.OrderBy(x => x.SubjectId);
                        break;
                    // Thêm các trường sort khác nếu cần
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

            return new PagedResult<ExamMatrix>
            {
                Total = total,
                Page = queryParams.Page,
                PageSize = queryParams.PageSize,
                Items = items
            };
        }
    }
}
