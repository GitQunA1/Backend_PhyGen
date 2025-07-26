using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IExamVersionRepo
    {
        Task<List<ExamVersion>> GetAllWithQueryAsync(ExamVersionQueryParameters parameters);
    }
}
