using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface ISubjectRepo : IBaseRepo<Subject, int>
    {
        Task<bool> SoftDelete(int id);
        Task<List<Subject>> GetActiveSubjectsAsync();
        Task<PagedResult<Subject>> GetAllWithQueryAsync(SubjectQueryParameters queryParams);
    }
}
