namespace ciit_api.DTOs.Student
{
    public class StudentAttendanceDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public int BatchId { get; set; }
        public string BatchName { get; set; }

        public int TopicId { get; set; }
        public string TopicName { get; set; }

        public int ContentId { get; set; }
        public string ContentName { get; set; }

        public DateTime? ExpectedDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public DateTime? AttendanceDate { get; set; }

        public int RegistrationId { get; set; }

        public int IsPresent { get; set; }
        public string Attendance { get; set; }
    }
}
