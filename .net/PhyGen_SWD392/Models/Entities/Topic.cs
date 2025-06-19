using PhyGen_SWD392.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhyGen_SWD392.Models.Entities;

public partial class Topic
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [Column("level")]
    public string? Level { get; set; }

    [NotMapped]
    public TopicLevelEnum? LevelEnum
    {
        get => Enum.TryParse<TopicLevelEnum>(Level, out var levelEnum) ? levelEnum : null;
        set => Level = value?.ToString();
    }

    public int? SubjectId { get; set; }

    public int? ParentId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Topic> InverseParent { get; set; } = new List<Topic>();

    public virtual ICollection<MatrixDetail> MatrixDetails { get; set; } = new List<MatrixDetail>();

    public virtual Topic? Parent { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual Subject? Subject { get; set; }
}
