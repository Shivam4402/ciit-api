using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Models;

public partial class CiitstudContext : DbContext
{
    public CiitstudContext()
    {
    }

    public CiitstudContext(DbContextOptions<CiitstudContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }

    public virtual DbSet<Tblbranch> Tblbranches { get; set; }

    public virtual DbSet<TbltopicContent> TbltopicContents { get; set; }

    public virtual DbSet<TbltrainingCourse> TbltrainingCourses { get; set; }

    public virtual DbSet<TbltrainingCourseFee> TbltrainingCourseFees { get; set; }

    public virtual DbSet<TbltrainingCourseTopic> TbltrainingCourseTopics { get; set; }

    public virtual DbSet<TbltrainingTopic> TbltrainingTopics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ciituser");

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.ToTable("AspNetUsers", "dbo");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetUserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.ToTable("AspNetUserRoles", "dbo");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserRoles).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Tblbranch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__tblbranc__E55E37DECF65ECE1");

            entity.ToTable("tblbranches", "dbo");

            entity.HasIndex(e => e.BranchName, "UQ__tblbranc__CF7A7E6C340F09F1").IsUnique();

            entity.HasIndex(e => e.BranchName, "UQ__tblbranc__CF7A7E6C395A9C14").IsUnique();

            entity.HasIndex(e => e.BranchName, "UQ__tblbranc__CF7A7E6C4A2BC2FE").IsUnique();

            entity.HasIndex(e => e.BranchName, "UQ__tblbranc__CF7A7E6C67B71DFB").IsUnique();

            entity.HasIndex(e => e.BranchName, "UQ__tblbranc__CF7A7E6C7398693B").IsUnique();

            entity.HasIndex(e => e.BranchName, "UQ__tblbranc__CF7A7E6C83A28242").IsUnique();

            entity.Property(e => e.BranchId).HasColumnName("branch_id");
            entity.Property(e => e.BranchName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("branch_name");
        });

        modelBuilder.Entity<TbltopicContent>(entity =>
        {
            entity.HasKey(e => e.ContentId).HasName("PK__tbltopic__655FE51070293DF5");

            entity.ToTable("tbltopic_contents", "dbo");

            entity.Property(e => e.ContentId).HasColumnName("content_id");
            entity.Property(e => e.ContentName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("content_name");
            entity.Property(e => e.Flag)
                .HasDefaultValue(0)
                .HasColumnName("flag");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");

            entity.HasOne(d => d.Topic).WithMany(p => p.TbltopicContents)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("fktopicid");
        });

        modelBuilder.Entity<TbltrainingCourse>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__tbltrain__8F1EF7AE1D85AA59");

            entity.ToTable("tbltraining_courses", "dbo");

            entity.HasIndex(e => e.CourseName, "UQ__tbltrain__B5B2A66A32A9DDE1").IsUnique();

            entity.HasIndex(e => e.CourseName, "UQ__tbltrain__B5B2A66A81A18235").IsUnique();

            entity.HasIndex(e => e.CourseName, "UQ__tbltrain__B5B2A66AA5AA539C").IsUnique();

            entity.HasIndex(e => e.CourseName, "UQ__tbltrain__B5B2A66AB7509A00").IsUnique();

            entity.HasIndex(e => e.CourseName, "UQ__tbltrain__B5B2A66AC588406D").IsUnique();

            entity.HasIndex(e => e.CourseName, "UQ__tbltrain__B5B2A66ADA81FD7D").IsUnique();

            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CourseName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("course_name");
            entity.Property(e => e.Flag)
                .HasDefaultValue(0)
                .HasColumnName("flag");
        });

        modelBuilder.Entity<TbltrainingCourseFee>(entity =>
        {
            entity.HasKey(e => e.FeeId).HasName("PK__tbltrain__A19C8AFB8453213F");

            entity.ToTable("tbltraining_course_fees", "dbo");

            entity.Property(e => e.FeeId).HasColumnName("fee_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.FeeMode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("fee_mode");
            entity.Property(e => e.FeesAmount).HasColumnName("fees_amount");
            entity.Property(e => e.FeesChangeDate).HasColumnName("fees_change_date");
            entity.Property(e => e.Flag)
                .HasDefaultValue(0)
                .HasColumnName("flag");
            entity.Property(e => e.Gst).HasColumnName("gst");

            entity.HasOne(d => d.Course).WithMany(p => p.TbltrainingCourseFees)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("fkcourseid");
        });

        modelBuilder.Entity<TbltrainingCourseTopic>(entity =>
        {
            entity.HasKey(e => e.CourseTopicId).HasName("PK__tbltrain__564DD4A3BEE47785");

            entity.ToTable("tbltraining_course_topics", "dbo");

            entity.Property(e => e.CourseTopicId).HasColumnName("course_topic_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.Flag)
                .HasDefaultValue(0)
                .HasColumnName("flag");
            entity.Property(e => e.TopicId).HasColumnName("topic_id");

            entity.HasOne(d => d.Course).WithMany(p => p.TbltrainingCourseTopics)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("fkcodi");

            entity.HasOne(d => d.Topic).WithMany(p => p.TbltrainingCourseTopics)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("fkcozdfdi");
        });

        modelBuilder.Entity<TbltrainingTopic>(entity =>
        {
            entity.HasKey(e => e.TopicId).HasName("PK__tbltrain__D5DAA3E90815FAA8");

            entity.ToTable("tbltraining_topics", "dbo");

            entity.HasIndex(e => e.TopicName, "UQ__tbltrain__54BAE5EC5F6DA262").IsUnique();

            entity.HasIndex(e => e.TopicName, "UQ__tbltrain__54BAE5EC8541C815").IsUnique();

            entity.HasIndex(e => e.TopicName, "UQ__tbltrain__54BAE5EC987F6ABA").IsUnique();

            entity.HasIndex(e => e.TopicName, "UQ__tbltrain__54BAE5ECAC4BCA78").IsUnique();

            entity.HasIndex(e => e.TopicName, "UQ__tbltrain__54BAE5ECC0075B4E").IsUnique();

            entity.HasIndex(e => e.TopicName, "UQ__tbltrain__54BAE5ECE59FE9C7").IsUnique();

            entity.Property(e => e.TopicId).HasColumnName("topic_id");
            entity.Property(e => e.Flag)
                .HasDefaultValue(0)
                .HasColumnName("flag");
            entity.Property(e => e.Publicfolderid)
                .IsUnicode(false)
                .HasColumnName("publicfolderid");
            entity.Property(e => e.TopicName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("topic_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
