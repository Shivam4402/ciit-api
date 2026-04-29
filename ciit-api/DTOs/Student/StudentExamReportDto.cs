namespace ciit_api.DTOs.Student
{
    public class StudentExamReportDto
    {
        public int ExamId { get; set; }
        public int RegistrationId { get; set; }

        public DateTime? ExamDate { get; set; }
        public string? StudentName { get; set; }
        public string? TopicName { get; set; }

        public string? StartTime { get; set; }
        public string? EndTime { get; set; }

        public int TotalQuestions { get; set; }
        public int CorrectQuestions { get; set; }
        public int WrongQuestions { get; set; }

        public decimal Percentage { get; set; }
        public string Grade { get; set; } = "Poor";
    }
}
