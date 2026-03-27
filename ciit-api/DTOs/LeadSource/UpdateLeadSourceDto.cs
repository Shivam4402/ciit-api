using System.ComponentModel.DataAnnotations;

namespace ciit_api.DTOs.LeadSource
{
    public class UpdateLeadSourceDto
    {
        [Required]
        public string SourceName { get; set; } = null!;
    }
}
