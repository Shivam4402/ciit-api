using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ciit_api.Models;

public partial class TbltrainingCourseTopic
{
    public int CourseTopicId { get; set; }

    public int? CourseId { get; set; }

    public int? TopicId { get; set; }

    public int? Flag { get; set; }

    public virtual TbltrainingCourse? Course { get; set; }

    public virtual TbltrainingTopic? Topic { get; set; }
}


public class CreateCourseTopicDto
{
    [Required]
    public int CourseId { get; set; }

    [Required]
    public int TopicId { get; set; }
}

public class UpdateCourseTopicDto
{
    [Required]
    public int CourseId { get; set; }

    [Required]
    public int TopicId { get; set; }
}

public class CourseTopicResponseDto
{
    public int CourseTopicId { get; set; }

    public int? CourseId { get; set; }

    public int? TopicId { get; set; }
}