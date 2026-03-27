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

    public virtual DbSet<Tblenquiry> Tblenquiries { get; set; }

    public virtual DbSet<TblenquiryFollowup> TblenquiryFollowups { get; set; }

    public virtual DbSet<TblenquiryFor> TblenquiryFors { get; set; }

    public virtual DbSet<TblleadSource> TblleadSources { get; set; }

    public virtual DbSet<Tblqualification> Tblqualifications { get; set; }

    public virtual DbSet<TblstudentDetail> TblstudentDetails { get; set; }

    public virtual DbSet<TblstudentPayment> TblstudentPayments { get; set; }

    public virtual DbSet<TblstudentQualification> TblstudentQualifications { get; set; }

    public virtual DbSet<TblstudentRegistration> TblstudentRegistrations { get; set; }

    public virtual DbSet<TbltopicContent> TbltopicContents { get; set; }

    public virtual DbSet<TbltrainingCourse> TbltrainingCourses { get; set; }

    public virtual DbSet<TbltrainingCourseFee> TbltrainingCourseFees { get; set; }

    public virtual DbSet<TbltrainingCourseTopic> TbltrainingCourseTopics { get; set; }

    public virtual DbSet<TbltrainingTopic> TbltrainingTopics { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=115.124.106.98;Database=ciitstud_;User Id=ciituser;Password=CIIT#0908;TrustServerCertificate=True");

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

            entity.HasOne(d => d.Branch).WithMany(p => p.Tblenquiries)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("fkbranch_id");
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

        modelBuilder.Entity<Tblqualification>(entity =>
        {
            entity.HasKey(e => e.QualificationId).HasName("PK__tblquali__CDACC5DBB1D32B76");

            entity.ToTable("tblqualifications", "dbo");

            entity.HasIndex(e => e.Qualification, "UQ__tblquali__33D617E50FE08E39").IsUnique();

            entity.HasIndex(e => e.Qualification, "UQ__tblquali__33D617E52CC222EE").IsUnique();

            entity.HasIndex(e => e.Qualification, "UQ__tblquali__33D617E54D25B8E6").IsUnique();

            entity.HasIndex(e => e.Qualification, "UQ__tblquali__33D617E584DFAE1B").IsUnique();

            entity.HasIndex(e => e.Qualification, "UQ__tblquali__33D617E5AC2709C3").IsUnique();

            entity.HasIndex(e => e.Qualification, "UQ__tblquali__33D617E5CBF3A920").IsUnique();

            entity.Property(e => e.QualificationId).HasColumnName("qualification_id");
            entity.Property(e => e.Flag)
                .HasDefaultValue(0)
                .HasColumnName("flag");
            entity.Property(e => e.Qualification)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("qualification");
        });

        modelBuilder.Entity<TblstudentDetail>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__tblstude__2A33069A7EB17965");

            entity.ToTable("tblstudent_details", "dbo");

            entity.HasIndex(e => e.EmailAddress, "UQ__tblstude__20C6DFF5798A24EB").IsUnique();

            entity.HasIndex(e => e.EmailAddress, "UQ__tblstude__20C6DFF5982AA4D5").IsUnique();

            entity.HasIndex(e => e.EmailAddress, "UQ__tblstude__20C6DFF5A7D6BC7B").IsUnique();

            entity.HasIndex(e => e.EmailAddress, "UQ__tblstude__20C6DFF5C7B054C4").IsUnique();

            entity.HasIndex(e => e.EmailAddress, "UQ__tblstude__20C6DFF5D61F4090").IsUnique();

            entity.HasIndex(e => e.EmailAddress, "UQ__tblstude__20C6DFF5D988DCA4").IsUnique();

            entity.HasIndex(e => e.PermanentIdentificationNumber, "UQ__tblstude__6B50C5DD00BAA003").IsUnique();

            entity.HasIndex(e => e.PermanentIdentificationNumber, "UQ__tblstude__6B50C5DD3C7D5C2E").IsUnique();

            entity.HasIndex(e => e.PermanentIdentificationNumber, "UQ__tblstude__6B50C5DD427111DC").IsUnique();

            entity.HasIndex(e => e.PermanentIdentificationNumber, "UQ__tblstude__6B50C5DD5705DADF").IsUnique();

            entity.HasIndex(e => e.PermanentIdentificationNumber, "UQ__tblstude__6B50C5DD698C8709").IsUnique();

            entity.HasIndex(e => e.PermanentIdentificationNumber, "UQ__tblstude__6B50C5DDC0A8CF5F").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.AadharCardNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("aadhar_card_number");
            entity.Property(e => e.AadharCardPhoto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("aadhar_card_photo");
            entity.Property(e => e.BirthDate)
                .HasColumnType("datetime")
                .HasColumnName("birth_date");
            entity.Property(e => e.BranchId).HasColumnName("branch_id");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email_address");
            entity.Property(e => e.Flag)
                .HasDefaultValue(0)
                .HasColumnName("flag");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.LocalAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("local_address");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("mobile_number");
            entity.Property(e => e.ParentName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("parent_name");
            entity.Property(e => e.ParentNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("parent_number");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PermanentAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("permanent_address");
            entity.Property(e => e.PermanentIdentificationNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("permanent_identification_number");
            entity.Property(e => e.ProfilePhoto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("profile_photo");
            entity.Property(e => e.Qualification)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("qualification");
            entity.Property(e => e.StudentCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("student_code");
            entity.Property(e => e.StudentName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("student_name");
            entity.Property(e => e.WhatsappNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("whatsapp_number");

            entity.HasOne(d => d.Branch).WithMany(p => p.TblstudentDetails)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("fkbranchstduentid");
        });

        modelBuilder.Entity<TblstudentPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__tblstude__ED1FC9EAC19C6201");

            entity.ToTable("tblstudent_payments", "dbo");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Flag).HasColumnName("flag");
            entity.Property(e => e.IsPaid)
                .HasDefaultValue(0)
                .HasColumnName("is_paid");
            entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount");
            entity.Property(e => e.PaymentDate)
                .HasColumnType("datetime")
                .HasColumnName("payment_date");
            entity.Property(e => e.PaymentDescription)
                .IsUnicode(false)
                .HasColumnName("payment_description");
            entity.Property(e => e.PaymentMode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("payment_mode");
            entity.Property(e => e.RegistrationId).HasColumnName("registration_id");

            entity.HasOne(d => d.Registration).WithMany(p => p.TblstudentPayments)
                .HasForeignKey(d => d.RegistrationId)
                .HasConstraintName("fkregid");
        });

        modelBuilder.Entity<TblstudentQualification>(entity =>
        {
            entity.HasKey(e => e.QualificationId).HasName("PK__tblstude__CDACC5DB4107B349");

            entity.ToTable("tblstudent_qualifications", "dbo");

            entity.Property(e => e.QualificationId).HasColumnName("qualification_id");
            entity.Property(e => e.Flag)
                .HasDefaultValue(0)
                .HasColumnName("flag");
            entity.Property(e => e.Medium)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("medium");
            entity.Property(e => e.PassingYear).HasColumnName("passing_year");
            entity.Property(e => e.Percentage).HasColumnName("percentage");
            entity.Property(e => e.Qualification)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("qualification");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.University)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("university");

            entity.HasOne(d => d.Student).WithMany(p => p.TblstudentQualifications)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("qid22");
        });

        modelBuilder.Entity<TblstudentRegistration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__tblstude__22A298F6C965F5C7");

            entity.ToTable("tblstudent_registrations", "dbo");

            entity.Property(e => e.RegistrationId).HasColumnName("registration_id");
            entity.Property(e => e.CurrentStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("current_status");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.FeeId).HasColumnName("fee_id");
            entity.Property(e => e.Flag)
                .HasDefaultValue(0)
                .HasColumnName("flag");
            entity.Property(e => e.RegistrationDate)
                .HasColumnType("datetime")
                .HasColumnName("registration_date");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Fee).WithMany(p => p.TblstudentRegistrations)
                .HasForeignKey(d => d.FeeId)
                .HasConstraintName("fkfeesid");

            entity.HasOne(d => d.Student).WithMany(p => p.TblstudentRegistrations)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("fkstidsd");
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
