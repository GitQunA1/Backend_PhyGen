using System;
using System.Collections.Generic;

namespace PhyGen_SWD392.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Grade { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<ExamMatrix> ExamMatrices { get; set; } = new List<ExamMatrix>();

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}
