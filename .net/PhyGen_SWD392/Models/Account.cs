using System;
using System.Collections.Generic;

namespace PhyGen_SWD392.Models;

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

    public virtual ICollection<ExamMatrix> ExamMatrices { get; set; } = new List<ExamMatrix>();

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
}
