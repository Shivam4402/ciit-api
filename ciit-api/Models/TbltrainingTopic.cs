using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class TbltrainingTopic
{
    public int TopicId { get; set; }

    public string TopicName { get; set; } = null!;

    public int? Flag { get; set; }

    public string? Publicfolderid { get; set; }

    public virtual ICollection<TblstudentExam> TblstudentExams { get; set; } = new List<TblstudentExam>();

    public virtual ICollection<TbltopicContent> TbltopicContents { get; set; } = new List<TbltopicContent>();

    public virtual ICollection<TbltrainingCourseTopic> TbltrainingCourseTopics { get; set; } = new List<TbltrainingCourseTopic>();
}
