using PhyGen_SWD392.Models.Enums;

namespace PhyGen_SWD392.Models.Requests
{
    public class ExamMatrixRequest
    {
        public int? SubjectId { get; set; }

        public ExamMatrixTypeEnum? ExamTypeEnum { get; set; }

        public ExamMatrixStatusEnum? StatusEnum { get; set; }
    }
}
