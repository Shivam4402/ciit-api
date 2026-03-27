using System.ComponentModel.DataAnnotations;

namespace ciit_api.DTOs.TopicContents
{
    public class UpdateTopicContentDto
    {
        [Required]
        public string ContentName { get; set; } = null!;
    }
}
