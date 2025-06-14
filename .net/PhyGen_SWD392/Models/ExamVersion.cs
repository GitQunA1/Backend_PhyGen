using System;
using System.Collections.Generic;

namespace PhyGen_SWD392.Models;

public partial class ExamVersion
{
    public int Id { get; set; }

    public int? FinalExamId { get; set; }

    public string? VersionCode { get; set; }

    public string? PdfUrl { get; set; }

    public string? QuestionsOrder { get; set; }

    public virtual Exam? FinalExam { get; set; }
}
