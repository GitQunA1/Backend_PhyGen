using PhyGen_SWD392.Models.DTOs;

namespace PhyGen_SWD392.Models.Requests
{
    public class AIExamRequest
    {
        public List<MatrixDetailDTO> MatrixDetails { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }
}
