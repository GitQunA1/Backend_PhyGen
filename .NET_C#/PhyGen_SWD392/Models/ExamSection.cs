using System;
using System.Collections.Generic;

namespace PhyGen_SWD392.Models;

public partial class ExamSection
{
    public int Id { get; set; }

    public int? ExamId { get; set; }

    public string? SectionName { get; set; }

    public int? DisplayOrder { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Exam? Exam { get; set; }

    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
}
