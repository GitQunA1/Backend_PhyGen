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

    public string? DifficultyLevel { get; set; }
    [NotMapped]
    public MatrixDetailLevelEnum? LevelEnum
    {
        get => Enum.TryParse<MatrixDetailLevelEnum>(DifficultyLevel, out var level) ? level : null;
        set => DifficultyLevel = value?.ToString();
    }

    public int? Quantity { get; set; }

    public string? Type { get; set; }
    [NotMapped]
    public MatrixDetailTypeEnum? TypeEnum
    {
        get => Enum.TryParse<MatrixDetailTypeEnum>(Type, out var type) ? type : null;
        set => Type = value?.ToString();
    }

    public virtual MatrixSection? MatrixSection { get; set; }

    public virtual Topic? Topic { get; set; }
}
