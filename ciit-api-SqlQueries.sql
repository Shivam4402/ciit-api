USE ciitstud_

select * from INFORMATION_SCHEMA.Tables;

SELECT *
FROM sys.procedures
WHERE name LIKE '%batch%';



SELECT *
FROM sys.procedures
WHERE name LIKE '%attendance%';


EXEC sp_helptext 'sp_student_wise_batches'
CREATE procedure [dbo].[sp_student_wise_batches](@student_id int)  
as  
begin  
   
 select batch_student_id,b.batch_id,batch_name,batch_time,tp.topic_id,topic_name,tr.employee_id,employee_name,start_date, r.registration_id,registration_date, s.student_id,student_name ,  
 c.course_id,course_name from  tblbatch_students bs join tblstudent_registrations r on bs.registration_id=r.registration_id  
join tblbatches b on bs.batch_id=b.batch_id  join tblstudent_details s on r.student_id=s.student_id  
join tbltraining_topics tp on tp.topic_id=b.topic_id join tblemployees tr on tr.employee_id=b.employee_id  
join tbltraining_course_fees f on r.fee_id=f.fee_id join  
tbltraining_courses c on f.course_id=c.course_id  
 where s.student_id=@student_id and r.flag=0 and s.flag=0 and f.flag=0 and b.flag=0 and bs.flag=0  
   
end;  


EXEC sp_helptext 'sp_fetch_student_wise_and_batch_wise_attendance'

CREATE procedure [dbo].[sp_fetch_student_wise_and_batch_wise_attendance](@batch_id  int , @registration_id int)  
as  
begin  
   
 select s.student_id,student_name, b.batch_id,batch_name,tp.topic_id, topic_name,tc.content_id, content_name,expected_date,actual_date,attendance_date,  
 r.registration_id,is_present ,  
 case is_present when  0 then 'Absent'  
 else 'Present'  
 end as 'attendance'  
   
 from   
 tblbatches b join tblbatch_schedule bsd on bsd.batch_id=b.batch_id  
join tbltopic_contents tc on tc.content_id=bsd.content_id  
join tblbatch_schedule_attendance bsa on bsd.batch_schedule_id=bsa.batch_schedule_id  
  join tblschedule_attendance sd on bsa.schedule_attendance_id=sd.schedule_attendance_id  
  join tblstudent_registrations r on r.registration_id=sd.registration_id  
  join tblstudent_details s on s.student_id=r.student_id  
  join tbltraining_topics tp on tp.topic_id=tc.topic_id  
where  b.batch_id=@batch_id  and actual_date is not null and   r.registration_id=@registration_id  
  
end  











select * from tblbatch_students
select * from tblbatches
select * from tbltraining_topics
select * from tbltopic_contents
select * from tbltraining_course_topics
select * from tbltraining_course_fees
select * from tblenquiries
select * from tblqualifications





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