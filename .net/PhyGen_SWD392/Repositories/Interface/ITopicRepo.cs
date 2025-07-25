using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface ITopicRepo : IBaseRepo<Topic, int>
    {
        Task<bool> SoftDelete(int id);
        Task<List<Topic>> GetActiveTopicsAsync();
        Task<PagedResult<Topic>> GetAllWithQueryAsync(TopicQueryParameters queryParams);
    }
}
