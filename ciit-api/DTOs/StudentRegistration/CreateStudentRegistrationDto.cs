namespace ciit_api.DTOs.StudentRegistration
{
    public class CreateStudentRegistrationDto
    {
        public int? StudentId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public double? Discount { get; set; }
        public int? FeeId { get; set; }
        public string? CurrentStatus { get; set; }
    }
}
