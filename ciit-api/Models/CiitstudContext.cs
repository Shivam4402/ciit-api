using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

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

    public virtual DbSet<Tblenquiry> Tblenquiries { get; set; }

    public virtual DbSet<TblenquiryFollowup> TblenquiryFollowups { get; set; }

    public virtual DbSet<TblenquiryFor> TblenquiryFors { get; set; }

    public virtual DbSet<TblleadSource> TblleadSources { get; set; }

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

        modelBuilder.Entity<Tblenquiry>(entity =>
        {
            entity.HasKey(e => e.EnquiryId).HasName("PK__tblenqui__57CC01B3BED7B890");

            entity.ToTable("tblenquiries", "dbo");

            entity.Property(e => e.EnquiryId).HasColumnName("enquiry_id");
            entity.Property(e => e.BirthDate)
                .HasColumnType("datetime")
                .HasColumnName("birth_date");
            entity.Property(e => e.BranchId).HasColumnName("branch_id");
            entity.Property(e => e.CandidateName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("candidate_name");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email_address");
            entity.Property(e => e.EnquiryDate)
                .HasColumnType("datetime")
                .HasColumnName("enquiry_date");
            entity.Property(e => e.EnquiryFors)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("enquiry_fors");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.InterestedTopics)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("interested_topics");
            entity.Property(e => e.LeadSources)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("lead_sources");
            entity.Property(e => e.LocalAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("local_address");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("mobile_number");
            entity.Property(e => e.Qualification)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("qualification");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
        });

        modelBuilder.Entity<TblenquiryFollowup>(entity =>
        {
            entity.HasKey(e => e.FollowupId).HasName("PK__tblenqui__6D23A5A19C0F46A4");

            entity.ToTable("tblenquiry_followups", "dbo");

            entity.Property(e => e.FollowupId).HasColumnName("followup_id");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EnquiryId).HasColumnName("enquiry_id");
            entity.Property(e => e.FollowUpBy)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("follow_up_by");
            entity.Property(e => e.FollowUpDate)
                .HasColumnType("datetime")
                .HasColumnName("follow_up_date");

            entity.HasOne(d => d.Enquiry).WithMany(p => p.TblenquiryFollowups)
                .HasForeignKey(d => d.EnquiryId)
                .HasConstraintName("fkenquid");
        });

        modelBuilder.Entity<TblenquiryFor>(entity =>
        {
            entity.HasKey(e => e.EnquiryForId).HasName("PK__tblenqui__20F32FC1356FE60A");

            entity.ToTable("tblenquiry_for", "dbo");

            entity.Property(e => e.EnquiryForId).HasColumnName("enquiry_for_id");
            entity.Property(e => e.EnquiryFor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("enquiry_for");
            entity.Property(e => e.Flag)
                .HasDefaultValue(0)
                .HasColumnName("flag");
        });

        modelBuilder.Entity<TblleadSource>(entity =>
        {
            entity.HasKey(e => e.SourceId).HasName("PK__tbllead___3035A9B6BB52AEF7");

            entity.ToTable("tbllead_sources", "dbo");

            entity.Property(e => e.SourceId).HasColumnName("source_id");
            entity.Property(e => e.Flag)
                .HasDefaultValue(0)
                .HasColumnName("flag");
            entity.Property(e => e.SourceName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("source_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }



    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
