using System.ComponentModel.DataAnnotations;

namespace ciit_api.DTOs.CourseTopic
{
    public class CreateCourseTopicDto
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        public int TopicId { get; set; }
    }
}
