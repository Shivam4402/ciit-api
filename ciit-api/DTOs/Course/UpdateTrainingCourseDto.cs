using System.ComponentModel.DataAnnotations;

namespace ciit_api.DTOs.Course
{
    public class UpdateTrainingCourseDto
    {
        [Required]
        public string CourseName { get; set; } = null!;

    }
}
