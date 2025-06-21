using PhyGen_SWD392.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhyGen_SWD392.Models.Entities;

public partial class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Grade { get; set; }

    [NotMapped]
    public SubjectGradeEnum? GradeEnum
    {
        get => Enum.TryParse(Grade, out SubjectGradeEnum grade) ? grade : (SubjectGradeEnum?)null;
        set => Grade = value?.ToString();
    }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<ExamMatrix> ExamMatrices { get; set; } = new List<ExamMatrix>();

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}
