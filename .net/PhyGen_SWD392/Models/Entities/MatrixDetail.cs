using PhyGen_SWD392.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhyGen_SWD392.Models.Entities;

public partial class MatrixDetail
{
    public int Id { get; set; }

    public int? MatrixSectionId { get; set; }

    public int? TopicId { get; set; }

    [Column("difficulty_level")]
    public string? DifficultyLevel { get; set; }

    [NotMapped]
    public MatrixDetailLevelEnum? DifficultyLevelId
    {
        get => Enum.TryParse<MatrixDetailLevelEnum>(DifficultyLevel, out var detailLevel) ? detailLevel : null;
        set => DifficultyLevel = value?.ToString();
    }

    public int? Quantity { get; set; }

    public string? Type { get; set; }

    public virtual MatrixSection? MatrixSection { get; set; }

    public virtual Topic? Topic { get; set; }
}
