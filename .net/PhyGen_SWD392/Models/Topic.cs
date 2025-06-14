using System;
using System.Collections.Generic;

namespace PhyGen_SWD392.Models;

public partial class Topic
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Level { get; set; }

    public int? SubjectId { get; set; }

    public int? ParentId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Topic> InverseParent { get; set; } = new List<Topic>();

    public virtual ICollection<MatrixDetail> MatrixDetails { get; set; } = new List<MatrixDetail>();

    public virtual Topic? Parent { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual Subject? Subject { get; set; }
}
