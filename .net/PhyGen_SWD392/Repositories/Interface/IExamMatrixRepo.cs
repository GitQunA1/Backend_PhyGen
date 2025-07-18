using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IExamMatrixRepo : IBaseRepo<ExamMatrix, int>
    {
        Task<bool> SoftDelete(int id);
        Task<List<ExamMatrix>> GetActiveExamMatricesAsync();
        Task<PagedResult<ExamMatrix>> GetAllWithQueryAsync(ExamMatrixQueryParameters queryParams);
    }
}
