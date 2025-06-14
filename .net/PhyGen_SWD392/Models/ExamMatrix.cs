using System;
using System.Collections.Generic;

namespace PhyGen_SWD392.Models;

public partial class ExamMatrix
{
    public int Id { get; set; }

    public int? SubjectId { get; set; }

    public int? CreatedBy { get; set; }

    public string? Examtype { get; set; }

    public string? Status { get; set; }

    public virtual Account? CreatedByNavigation { get; set; }

    public virtual ICollection<MatrixSection> MatrixSections { get; set; } = new List<MatrixSection>();

    public virtual Subject? Subject { get; set; }
}
