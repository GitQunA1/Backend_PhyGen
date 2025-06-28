using PhyGen_SWD392.Models.Entities;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface ISubjectRepo : IBaseRepo<Subject, int>
    {
        Task<bool> SoftDelete(int id);
        Task<List<Subject>> GetActiveSubjectsAsync();
    }
}
