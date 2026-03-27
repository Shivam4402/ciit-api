using System.ComponentModel.DataAnnotations;

namespace ciit_api.DTOs.Topic
{
    public class UpdateTrainingTopicDto
    {
        [Required]
        public string TopicName { get; set; } = null!;
    }
}
