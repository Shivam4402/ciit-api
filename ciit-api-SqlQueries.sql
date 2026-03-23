USE ciitstud_

select * from INFORMATION_SCHEMA.Tables;

select * from tbltraining_topics
select * from tbltopic_contents
select * from tbltraining_course_topics
select * from tbltraining_course_fees
select * from tblenquiries


select * from tblbranches
select * from tbltraining_courses

select * from AspNetUsers
select * from AspNetUserRoles
select * from AspNetUserTokens
select * from AspNetUserClaims



Server=115.124.106.98;Database=ciitstud_;User Id=ciituser;Password=CIIT#0908;TrustServerCertificate=True


Perform CRUD Operations
Course wise fees
course wise topics
course wise topic & topic wise contents
topic wise contents



Select * from tbllead_sources
Select * from tblenquiry_for
Select * from tbltraining_topics
Select * from tblenquiries
Select * from tblenquiry_followups




Scaffold-DbContext "Server=115.124.106.98;Database=ciitstud_;User Id=ciituser;Password=CIIT#0908;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Table tbllead_sources, tblenquiry_for, tblenquiries, tblenquiry_followups


CREATE TABLE TblEnquiryLeadSource
(
    Id INT PRIMARY KEY IDENTITY(1,1),

    EnquiryId INT,
    LeadSourceId INT,

    FOREIGN KEY (EnquiryId) REFERENCES TblEnquiry(EnquiryId),
    FOREIGN KEY (LeadSourceId) REFERENCES TblLeadSource(LeadSourceId)
);