using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PhyGen_SWD392.Models;

public partial class PhyGenDbContext : DbContext
{
    public PhyGenDbContext()
    {
    }

    public PhyGenDbContext(DbContextOptions<PhyGenDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamMatrix> ExamMatrices { get; set; }

    public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }

    public virtual DbSet<ExamSection> ExamSections { get; set; }

    public virtual DbSet<ExamVersion> ExamVersions { get; set; }

    public virtual DbSet<MatrixDetail> MatrixDetails { get; set; }

    public virtual DbSet<MatrixSection> MatrixSections { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=aws-0-ap-southeast-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.kbicikwvyuseaqrfzkqq;Password=swd392;Ssl Mode=Require;Trust Server Certificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("account_pkey");

            entity.ToTable("account");

            entity.HasIndex(e => e.Email, "account_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "account_username_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountType)
                .HasDefaultValueSql("'free'::text")
                .HasColumnName("account_type");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.EmailVerified)
                .HasDefaultValue(false)
                .HasColumnName("email_verified");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasDefaultValueSql("'user'::text")
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'active'::text")
                .HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("answers_pkey");

            entity.ToTable("answers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.IsCorrect)
                .HasDefaultValue(false)
                .HasColumnName("is_correct");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("answers_question_id_fkey");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("exam_pkey");

            entity.ToTable("exam");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Examtype).HasColumnName("examtype");
            entity.Property(e => e.IsDraft).HasColumnName("is_draft");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'active'::text")
                .HasColumnName("status");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Exams)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("exam_created_by_fkey");
        });

        modelBuilder.Entity<ExamMatrix>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("exam_matrices_pkey");

            entity.ToTable("exam_matrices");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Examtype).HasColumnName("examtype");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'active'::text")
                .HasColumnName("status");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ExamMatrices)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("exam_matrices_created_by_fkey");

            entity.HasOne(d => d.Subject).WithMany(p => p.ExamMatrices)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("exam_matrices_subject_id_fkey");
        });

        modelBuilder.Entity<ExamQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("exam_questions_pkey");

            entity.ToTable("exam_questions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExamId).HasColumnName("exam_id");
            entity.Property(e => e.OrderIndex).HasColumnName("order_index");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.SectionId).HasColumnName("section_id");

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamQuestions)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("exam_questions_exam_id_fkey");

            entity.HasOne(d => d.Question).WithMany(p => p.ExamQuestions)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("exam_questions_question_id_fkey");

            entity.HasOne(d => d.Section).WithMany(p => p.ExamQuestions)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("exam_questions_section_id_fkey");
        });

        modelBuilder.Entity<ExamSection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("exam_sections_pkey");

            entity.ToTable("exam_sections");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.ExamId).HasColumnName("exam_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.SectionName)
                .HasMaxLength(100)
                .HasColumnName("section_name");

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamSections)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("exam_sections_exam_id_fkey");
        });

        modelBuilder.Entity<ExamVersion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("exam_versions_pkey");

            entity.ToTable("exam_versions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FinalExamId).HasColumnName("final_exam_id");
            entity.Property(e => e.PdfUrl).HasColumnName("pdf_url");
            entity.Property(e => e.QuestionsOrder).HasColumnName("questions_order");
            entity.Property(e => e.VersionCode)
                .HasMaxLength(10)
                .HasColumnName("version_code");

            entity.HasOne(d => d.FinalExam).WithMany(p => p.ExamVersions)
                .HasForeignKey(d => d.FinalExamId)
                .HasConstraintName("exam_versions_final_exam_id_fkey");
        });

        modelBuilder.Entity<MatrixDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("matrix_details_pkey");

            entity.ToTable("matrix_details");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DifficultyLevel).HasColumnName("difficulty_level");
            entity.Property(e => e.MatrixSectionId).HasColumnName("matrix_section_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.MatrixSection).WithMany(p => p.MatrixDetails)
                .HasForeignKey(d => d.MatrixSectionId)
                .HasConstraintName("matrix_details_matrix_section_id_fkey");

            entity.HasOne(d => d.Topic).WithMany(p => p.MatrixDetails)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("matrix_details_topic_id_fkey");
        });

        modelBuilder.Entity<MatrixSection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("matrix_sections_pkey");

            entity.ToTable("matrix_sections");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.MatrixId).HasColumnName("matrix_id");
            entity.Property(e => e.SectionName)
                .HasMaxLength(100)
                .HasColumnName("section_name");

            entity.HasOne(d => d.Matrix).WithMany(p => p.MatrixSections)
                .HasForeignKey(d => d.MatrixId)
                .HasConstraintName("matrix_sections_matrix_id_fkey");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("questions_pkey");

            entity.ToTable("questions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.DifficultyLevel)
                .HasDefaultValueSql("'medium'::text")
                .HasColumnName("difficulty_level");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'active'::text")
                .HasColumnName("status");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.Topic).WithMany(p => p.Questions)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("questions_topic_id_fkey");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("subject_pkey");

            entity.ToTable("subject");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("topics_pkey");

            entity.ToTable("topics");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("topics_parent_id_fkey");

            entity.HasOne(d => d.Subject).WithMany(p => p.Topics)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("topics_subject_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
