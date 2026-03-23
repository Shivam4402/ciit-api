using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ciit_api.Models;

public partial class TbltopicContent
{
    public int ContentId { get; set; }

    public int? TopicId { get; set; }

    public string? ContentName { get; set; }

    public int? Flag { get; set; }

    public virtual TbltrainingTopic? Topic { get; set; }
}

public class CreateTopicContentDto
{
    [Required]
    public int TopicId { get; set; }

    [Required]
    public string ContentName { get; set; } = null!;
}

public class UpdateTopicContentDto
{
    [Required]
    public string ContentName { get; set; } = null!;
}

public class TopicContentResponseDto
{
    public int ContentId { get; set; }

    public int? TopicId { get; set; }

    public string? ContentName { get; set; }
}