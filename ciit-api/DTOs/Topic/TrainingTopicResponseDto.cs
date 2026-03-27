namespace ciit_api.DTOs.Topic
{
    public class TrainingTopicResponseDto
    {
        public int TopicId { get; set; }

        public string TopicName { get; set; } = null!;

        public string? Publicfolderid { get; set; }
    }
}
