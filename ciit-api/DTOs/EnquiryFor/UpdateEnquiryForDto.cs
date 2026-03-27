using System.ComponentModel.DataAnnotations;

namespace ciit_api.DTOs.EnquiryFor
{
    public class UpdateEnquiryForDto
    {
        [Required]
        public string EnquiryFor { get; set; } = null!;
    }

}
