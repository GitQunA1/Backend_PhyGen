using PhyGen_SWD392.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhyGen_SWD392.Models.Entities;

public partial class ExamMatrix
{
    public int Id { get; set; }

    public int? SubjectId { get; set; }

    public int? CreatedBy { get; set; }

    public string? Examtype { get; set; }
    [NotMapped]
    public ExamMatrixTypeEnum? ExamTypeEnum
    {
        get => Enum.TryParse(Examtype, out ExamMatrixTypeEnum examType) ? examType : (ExamMatrixTypeEnum?)null;
        set => Examtype = value?.ToString();
    }

    public string? Status { get; set; }
    [NotMapped]
    public ExamMatrixStatusEnum? StatusEnum
    {
        get => Enum.TryParse(Status, out ExamMatrixStatusEnum status) ? status : (ExamMatrixStatusEnum?)null;
        set => Status = value?.ToString();
    }

    public virtual Account? CreatedByNavigation { get; set; }

    public virtual ICollection<MatrixSection> MatrixSections { get; set; } = new List<MatrixSection>();

    public virtual Subject? Subject { get; set; }
}
