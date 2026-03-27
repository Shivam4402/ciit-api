USE ciitstud_

select * from INFORMATION_SCHEMA.Tables;

select * from tbltraining_topics
select * from tbltopic_contents
select * from tbltraining_course_topics
select * from tbltraining_course_fees
select * from tblenquiries

select * from tblqualifications

DELETE FROM tblenquiries WHERE enquiry_id = 2314



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




Scaffold-DbContext "Server=115.124.106.98;Database=ciitstud_;User Id=ciituser;Password=CIIT#0908;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Table AspNetUsers, AspNetUserRoles, tbltraining_courses, tblbranches, tblenquiries, tbltraining_course_fees, tbltraining_course_topics, tbltraining_topics, tbltopic_contents, tbllead_sources, tblenquiry_for, tblenquiry_followups, tblqualifications, tblstudent_details, tblstudent_registrations, tblstudent_qualifications, tblstudent_payments -force


SELECT UserName, Email FROM AspNetUsers



select * from tblstudent_details
select * from tblstudent_registrations
select * from tblstudent_qualifications
select * from tblstudent_payments



yuvraj.gadadare@gmail.com
P0wersh@t