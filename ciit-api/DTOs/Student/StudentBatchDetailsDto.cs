namespace ciit_api.DTOs.Student
{
    public class StudentBatchDetailsDto
    {
        public int BatchStudentId { get; set; }
        public int BatchId { get; set; }
        public string BatchName { get; set; }
        public string BatchTime { get; set; }

        public int TopicId { get; set; }
        public string TopicName { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public DateTime StartDate { get; set; }

        public int RegistrationId { get; set; }
        public DateTime RegistrationDate { get; set; }

        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
}
