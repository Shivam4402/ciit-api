using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class TblstudentExam
{
    public int ExamId { get; set; }

    public int? StudentId { get; set; }

    public DateTime? ExamDate { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? TopicId { get; set; }

    public string? Status { get; set; }

    public int? TotalQuestions { get; set; }

    public virtual TblstudentDetail? Student { get; set; }

    public virtual ICollection<TblstudentExamQuestion> TblstudentExamQuestions { get; set; } = new List<TblstudentExamQuestion>();

    public virtual TbltrainingTopic? Topic { get; set; }
}
