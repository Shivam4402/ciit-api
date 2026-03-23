using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ciit_api.Models;

public partial class TbltrainingTopic
{
    public int TopicId { get; set; }

    public string TopicName { get; set; } = null!;
     
    public int? Flag { get; set; }

    public string? Publicfolderid { get; set; }

    public virtual ICollection<TbltopicContent> TbltopicContents { get; set; } = new List<TbltopicContent>();

    public virtual ICollection<TbltrainingCourseTopic> TbltrainingCourseTopics { get; set; } = new List<TbltrainingCourseTopic>();
}


public class CreateTrainingTopicDto
{
    [Required]
    public string TopicName { get; set; } = null!;
}

public class UpdateTrainingTopicDto
{
    [Required]
    public string TopicName { get; set; } = null!;
}

public class TrainingTopicResponseDto
{
    public int TopicId { get; set; }

    public string TopicName { get; set; } = null!;

    public string? Publicfolderid { get; set; }
}

public class TopicWithContentsResponseDto
{
    public int TopicId { get; set; }
    public string TopicName { get; set; } = null!;
    public List<TopicContentResponseDto> Contents { get; set; } = new();
}