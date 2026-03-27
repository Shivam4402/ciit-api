using ciit_api.DTOs.TopicContents;

namespace ciit_api.DTOs.Topic
{
    public class TopicWithContentsResponseDto
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; } = null!;
        public List<TopicContentResponseDto> Contents { get; set; } = new();
    }
}
