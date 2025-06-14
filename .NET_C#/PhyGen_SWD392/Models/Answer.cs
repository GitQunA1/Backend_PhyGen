using System;
using System.Collections.Generic;

namespace PhyGen_SWD392.Models;

public partial class Answer
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public bool? IsCorrect { get; set; }

    public int? QuestionId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Question? Question { get; set; }
}
