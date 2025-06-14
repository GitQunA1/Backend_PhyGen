using System;
using System.Collections.Generic;

namespace PhyGen_SWD392.Models;

public partial class MatrixSection
{
    public int Id { get; set; }

    public int? MatrixId { get; set; }

    public string? SectionName { get; set; }

    public int? DisplayOrder { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ExamMatrix? Matrix { get; set; }

    public virtual ICollection<MatrixDetail> MatrixDetails { get; set; } = new List<MatrixDetail>();
}
