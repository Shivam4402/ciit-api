using System.ComponentModel.DataAnnotations;

namespace ciit_api.DTOs.TopicContents
{
    public class CreateTopicContentDto
    {
        [Required]
        public int TopicId { get; set; }

        [Required]
        public string ContentName { get; set; } = null!;
    }

}
