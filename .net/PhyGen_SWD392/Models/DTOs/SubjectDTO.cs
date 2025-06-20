namespace PhyGen_SWD392.Models.DTOs
{
    public class SubjectDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Grade { get; set; }
    }
}
