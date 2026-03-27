using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class TbltrainingCourse
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int? Flag { get; set; }

    public virtual ICollection<TbltrainingCourseFee> TbltrainingCourseFees { get; set; } = new List<TbltrainingCourseFee>();

    public virtual ICollection<TbltrainingCourseTopic> TbltrainingCourseTopics { get; set; } = new List<TbltrainingCourseTopic>();
}
