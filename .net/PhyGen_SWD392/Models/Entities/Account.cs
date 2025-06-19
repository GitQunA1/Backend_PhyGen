using PhyGen_SWD392.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhyGen_SWD392.Models.Entities;

public partial class Account
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool? EmailVerified { get; set; }

    public string? Role { get; set; }

    public string? AccountType { get; set; }

    public string? Status { get; set; }

    [NotMapped]
    public RoleEnum? RoleEnum
    {
        get => Enum.TryParse<RoleEnum>(Role, out var roleEnum) ? roleEnum : null;
        set => Role = value?.ToString();
    }

    [NotMapped]
    public AccountTypeEnum? AccountTypeEnum
    {
        get => Enum.TryParse<AccountTypeEnum>(AccountType, out var typeEnum) ? typeEnum : null;
        set => AccountType = value?.ToString();
    }

    [NotMapped]
    public AccountStatusEnum? StatusEnum
    {
        get => Enum.TryParse<AccountStatusEnum>(Status, out var statusEnum) ? statusEnum : null;
        set => Status = value?.ToString();
    }

    public virtual ICollection<ExamMatrix> ExamMatrices { get; set; } = new List<ExamMatrix>();

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
}
