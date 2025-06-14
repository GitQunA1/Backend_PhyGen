using System;
using System.Collections.Generic;

namespace PhyGen_SWD392.Models;

public partial class MatrixDetail
{
    public int Id { get; set; }

    public int? MatrixSectionId { get; set; }

    public int? TopicId { get; set; }

    public string? DifficultyLevel { get; set; }

    public int? Quantity { get; set; }

    public string? Type { get; set; }

    public virtual MatrixSection? MatrixSection { get; set; }

    public virtual Topic? Topic { get; set; }
}
