using ciit_api.DTOs.Student;
using ciit_api.Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ciit_api.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IConfiguration _config;

        public StudentService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<StudentBatchDetailsDto>> GetStudentWiseBatchDetails(int studentId)
        {
            var list = new List<StudentBatchDetailsDto>();

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("sp_student_wise_batches", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@student_id", studentId);

                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new StudentBatchDetailsDto
                            {
                                BatchStudentId = Convert.ToInt32(reader["batch_student_id"]),
                                BatchId = Convert.ToInt32(reader["batch_id"]),
                                BatchName = reader["batch_name"].ToString(),
                                BatchTime = reader["batch_time"].ToString(),

                                TopicId = Convert.ToInt32(reader["topic_id"]),
                                TopicName = reader["topic_name"].ToString(),

                                EmployeeId = Convert.ToInt32(reader["employee_id"]),
                                EmployeeName = reader["employee_name"].ToString(),

                                StartDate = Convert.ToDateTime(reader["start_date"]),

                                RegistrationId = Convert.ToInt32(reader["registration_id"]),
                                RegistrationDate = Convert.ToDateTime(reader["registration_date"]),

                                StudentId = Convert.ToInt32(reader["student_id"]),
                                StudentName = reader["student_name"].ToString(),

                                CourseId = Convert.ToInt32(reader["course_id"]),
                                CourseName = reader["course_name"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }


        public async Task<List<StudentAttendanceDto>> GetStudentBatchWiseAttendance(int batchId, int registrationId)
        {
            var list = new List<StudentAttendanceDto>();

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("sp_fetch_student_wise_and_batch_wise_attendance", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = batchId;
                    cmd.Parameters.Add("@registration_id", SqlDbType.Int).Value = registrationId;

                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new StudentAttendanceDto
                            {
                                StudentId = Convert.ToInt32(reader["student_id"]),
                                StudentName = reader["student_name"].ToString(),

                                BatchId = Convert.ToInt32(reader["batch_id"]),
                                BatchName = reader["batch_name"].ToString(),

                                TopicId = Convert.ToInt32(reader["topic_id"]),
                                TopicName = reader["topic_name"].ToString(),

                                ContentId = Convert.ToInt32(reader["content_id"]),
                                ContentName = reader["content_name"].ToString(),

                                ExpectedDate = reader["expected_date"] == DBNull.Value ? null : Convert.ToDateTime(reader["expected_date"]),
                                ActualDate = reader["actual_date"] == DBNull.Value ? null : Convert.ToDateTime(reader["actual_date"]),
                                AttendanceDate = reader["attendance_date"] == DBNull.Value ? null : Convert.ToDateTime(reader["attendance_date"]),

                                RegistrationId = Convert.ToInt32(reader["registration_id"]),

                                IsPresent = Convert.ToInt32(reader["is_present"]),
                                Attendance = reader["attendance"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }


        public async Task<List<StudentBatchExamDto>> GetStudentWiseBatchExams(int registrationId)
        {
            var list = new List<StudentBatchExamDto>();

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("sp_fetch_student_wise_batch_exams", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@registration_id", SqlDbType.Int).Value = registrationId;

                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new StudentBatchExamDto
                            {
                                ExamId = Convert.ToInt32(reader["exam_id"]),
                                RegistrationId = Convert.ToInt32(reader["registration_id"]),

                                StudentId = Convert.ToInt32(reader["student_id"]),
                                StudentName = reader["student_name"].ToString(),

                                TopicId = Convert.ToInt32(reader["topic_id"]),
                                TopicName = reader["topic_name"].ToString(),

                                BatchId = Convert.ToInt32(reader["batch_id"]),
                                BatchName = reader["batch_name"].ToString(),

                                ExamDate = reader["exam_date"] == DBNull.Value ? null : Convert.ToDateTime(reader["exam_date"]),

                                StartTime = reader["start_time"]?.ToString(),
                                EndTime = reader["end_time"]?.ToString(),

                                TotalQuestions = Convert.ToInt32(reader["total_questions"]),

                                IsAttended = Convert.ToInt32(reader["is_attended"])
                            });
                        }
                    }
                }
            }

            return list;
        }
    }
}
