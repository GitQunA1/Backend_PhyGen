using PhyGen_SWD392.Models.Enums;

namespace PhyGen_SWD392.Models.Requests
{
    public class SubjectRequest
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public SubjectGradeEnum? GradeEnum { get; set; }
    }
}
