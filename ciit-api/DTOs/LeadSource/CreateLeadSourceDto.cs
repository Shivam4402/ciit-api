using System.ComponentModel.DataAnnotations;

namespace ciit_api.DTOs.LeadSource
{
    public class CreateLeadSourceDto
    {
        [Required]
        public string SourceName { get; set; } = null!;
    }
}
