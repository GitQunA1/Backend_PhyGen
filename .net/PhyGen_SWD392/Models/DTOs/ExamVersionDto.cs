namespace PhyGen_SWD392.Models.DTOs
{
    public class ExamVersionDto
    {
        public int Id { get; set; }

        public int? FinalExamId { get; set; }

        public string? VersionCode { get; set; }

        public string? PdfUrl { get; set; }
    }
}
