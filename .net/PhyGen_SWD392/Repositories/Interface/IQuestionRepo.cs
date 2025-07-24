using PhyGen_SWD392.Models.Entities;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IQuestionRepo : IBaseRepo<Question, int>
    {
        Task<List<Question>> GetQuestionsForMatrixDetailAsync(List<MatrixDetail> details);
    }
}
