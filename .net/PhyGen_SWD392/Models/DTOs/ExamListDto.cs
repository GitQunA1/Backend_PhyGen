namespace PhyGen_SWD392.Models.DTOs
{
    public class ExamListDto
    {
        public int Id { get; set; }
        public bool? IsDraft { get; set; }
        public string? ExamType { get; set; }
        public DateTime? CreatedAt { get; set; }

        public int QuestionCount { get; set; }
        public string? SubjectName { get; set; }
    }
}
