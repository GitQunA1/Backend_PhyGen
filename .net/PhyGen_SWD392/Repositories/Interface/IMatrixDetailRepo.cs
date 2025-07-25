using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IMatrixDetailRepo : IBaseRepo<MatrixDetail, int>
    {
        Task<PagedResult<MatrixDetail>> GetAllWithQueryAsync(MatrixDetailQueryParameters queryParams);
    }
}
