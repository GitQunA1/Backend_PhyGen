namespace PhyGen_SWD392.Models.DTOs
{
    public class TopicDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Level { get; set; }
        public int? SubjectId { get; set; }
        public int? ParentId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
