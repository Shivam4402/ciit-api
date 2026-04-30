namespace ciit_api.DTOs.StudentRegistration
{
    public class StudentRegistrationResponseDto
    {
        public int RegistrationId { get; set; }
        public int? StudentId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public double? Discount { get; set; }
        public int? FeeId { get; set; }
        public string? CurrentStatus { get; set; }
        public string? StudentName { get; set; }
        public int? CourseId { get; set; }
        public string? CourseName { get; set; }
    }
}
