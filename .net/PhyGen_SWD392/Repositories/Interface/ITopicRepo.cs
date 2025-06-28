using PhyGen_SWD392.Models.Entities;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface ITopicRepo : IBaseRepo<Topic, int>
    {
        Task<bool> SoftDelete(int id);
        Task<List<Topic>> GetActiveTopicsAsync();
    }
}
