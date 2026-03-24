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



Scaffold-DbContext "Server=115.124.106.98;Database=ciitstud_;User Id=ciituser;Password=CIIT#0908;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir TempModels -Table AspNetUsers, AspNetUserRoles, tbltraining_courses, tblbranches, tblenquiries, tbltraining_course_fees, tbltraining_course_topics, tbltraining_topics, tbltopic_contents, tbllead_sources, tblenquiry_for, tblenquiry_followups, tblqualifications -force


CREATE TABLE tbllead_sources_map
(
    Id INT PRIMARY KEY IDENTITY(1,1),

    enquiry_id INT,
    source_id INT,

    FOREIGN KEY (enquiry_id) REFERENCES tblenquiries(enquiry_id),
    FOREIGN KEY (source_id) REFERENCES tbllead_sources(source_id)
);

CREATE TABLE tblenquiry_for_map
(
    Id INT PRIMARY KEY IDENTITY(1,1),

    enquiry_id INT,
    enquiry_for_id INT,

    FOREIGN KEY (enquiry_id) REFERENCES tblenquiries(enquiry_id),
    FOREIGN KEY (enquiry_for_id) REFERENCES tblenquiry_for(enquiry_for_id)
);

CREATE TABLE tbl_enquiry_topic_map
(
    Id INT PRIMARY KEY IDENTITY(1,1),

    enquiry_id INT,
    topic_id INT,

    FOREIGN KEY (enquiry_id) REFERENCES tblenquiries(enquiry_id),
    FOREIGN KEY (topic_id) REFERENCES tbltraining_topics(topic_id)
);


SELECT UserName, Email FROM AspNetUsers