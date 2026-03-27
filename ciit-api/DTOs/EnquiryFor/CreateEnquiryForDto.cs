using System.ComponentModel.DataAnnotations;

namespace ciit_api.DTOs.EnquiryFor
{
    public class CreateEnquiryForDto
    {
        [Required]
        public string EnquiryFor { get; set; } = null!;
    }
}
