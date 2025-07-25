namespace PhyGen_SWD392.Models.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string? DifficultyLevel { get; set; }

        public int? TopicId { get; set; }

        public string? Status { get; set; }
    }
}
