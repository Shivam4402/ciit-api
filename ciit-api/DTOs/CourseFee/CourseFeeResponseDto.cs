namespace ciit_api.DTOs.CourseFee
{
    public class CourseFeeResponseDto
    {
        public int FeeId { get; set; }

        public int? CourseId { get; set; }

        public double FeesAmount { get; set; }

        public double? Gst { get; set; }

        public string? FeeMode { get; set; }

        public DateOnly? FeesChangeDate { get; set; }
    }
}
