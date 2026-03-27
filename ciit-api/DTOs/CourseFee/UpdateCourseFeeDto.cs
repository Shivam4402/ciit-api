using System.ComponentModel.DataAnnotations;

namespace ciit_api.DTOs.CourseFee
{
    public class UpdateCourseFeeDto
    {
        [Required]
        public double FeesAmount { get; set; }

        public double? Gst { get; set; }

        public string? FeeMode { get; set; }

    }
}
