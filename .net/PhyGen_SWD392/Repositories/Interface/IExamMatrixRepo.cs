using PhyGen_SWD392.Models.Entities;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IExamMatrixRepo : IBaseRepo<ExamMatrix, int>
    {
        Task<bool> SoftDelete(int id);
        Task<List<ExamMatrix>> GetActiveExamMatricesAsync();
    }
}
