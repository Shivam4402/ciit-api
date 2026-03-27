using System.ComponentModel.DataAnnotations;

namespace ciit_api.DTOs.Course
{
    public class CreateTrainingCourseDto
    {
        [Required]
        public string CourseName { get; set; } = null!;

    }

}
