using System;
using System.Collections.Generic;

namespace PhyGen_SWD392.Models;

public partial class Question
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? DifficultyLevel { get; set; }

    public int? TopicId { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();

    public virtual Topic? Topic { get; set; }
}
