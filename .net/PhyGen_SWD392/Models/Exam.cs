using System;
using System.Collections.Generic;

namespace PhyGen_SWD392.Models;

public partial class Exam
{
    public int Id { get; set; }

    public int? CreatedBy { get; set; }

    public bool? IsDraft { get; set; }

    public string? Examtype { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public virtual Account? CreatedByNavigation { get; set; }

    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();

    public virtual ICollection<ExamSection> ExamSections { get; set; } = new List<ExamSection>();

    public virtual ICollection<ExamVersion> ExamVersions { get; set; } = new List<ExamVersion>();
}
