using System;
using System.Collections.Generic;

namespace PhyGen_SWD392.Models;

public partial class ExamQuestion
{
    public int Id { get; set; }

    public int? ExamId { get; set; }

    public int? QuestionId { get; set; }

    public int? SectionId { get; set; }

    public int? OrderIndex { get; set; }

    public virtual Exam? Exam { get; set; }

    public virtual Question? Question { get; set; }

    public virtual ExamSection? Section { get; set; }
}
