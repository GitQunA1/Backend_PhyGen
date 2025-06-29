namespace PhyGen_SWD392.Models.DTOs
{
    public class MatrixDetailDTO
    {
        public int Id { get; set; }
        public int? MatrixSectionId { get; set; }
        public string? SectionName { get; set; }
        public int? TopicId { get; set; }
        public string? TopicName { get; set; }
        public string? DifficultyLevel { get; set; }
        public int? Quantity { get; set; }
        public string? Type { get; set; }
    }
}
