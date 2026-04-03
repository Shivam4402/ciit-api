namespace ciit_api.DTOs.CourseFee
{
    public class PaymentDto
    {
        public string PaymentDate { get; set; }
        public string PaymentMode { get; set; }
        public decimal Amount { get; set; }
    }

    public class CourseFeePdfDto
    {
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public string RegistrationDate { get; set; }
        public string Status { get; set; }
        public string FeeMode { get; set; }

        public decimal TotalFee { get; set; }
        public decimal Discount { get; set; }
        public decimal PayableFee { get; set; }
        public decimal PaidFee { get; set; }
        public decimal DueFee { get; set; }

        public List<PaymentDto> Payments { get; set; }
    }
}
