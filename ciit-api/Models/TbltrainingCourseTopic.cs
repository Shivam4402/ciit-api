using System;
using System.Collections.Generic;

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
