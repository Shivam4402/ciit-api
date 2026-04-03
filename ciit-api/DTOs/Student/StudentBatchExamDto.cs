namespace ciit_api.DTOs.Student
{
    public class StudentBatchExamDto
    {
        public int ExamId { get; set; }

        public int RegistrationId { get; set; }

        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public int TopicId { get; set; }
        public string TopicName { get; set; }

        public int BatchId { get; set; }
        public string BatchName { get; set; }

        public DateTime? ExamDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public int TotalQuestions { get; set; }

        public int IsAttended { get; set; } 
    }
}
