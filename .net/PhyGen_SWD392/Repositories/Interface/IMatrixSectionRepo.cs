using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IMatrixSectionRepo : IBaseRepo<MatrixSection, int>
    {
        Task<bool> SoftDelete(int id);
        Task<List<MatrixSection>> GetActiveMatrixSectionsAsync();
        Task<PagedResult<MatrixSection>> GetAllWithQueryAsync(MatrixSectionQueryParameters queryParams);
    }
}
