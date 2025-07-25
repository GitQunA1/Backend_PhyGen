namespace PhyGen_SWD392.Models.DTOs
{
    public class ExamMatrixDTO
    {
        public int Id { get; set; }

        public int? SubjectId { get; set; }

        public int? CreatedBy { get; set; }

        public string? Examtype { get; set; }

        public string? Status { get; set; }
    }
}
