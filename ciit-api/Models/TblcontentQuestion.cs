using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class TblcontentQuestion
{
    public int QuestionId { get; set; }

    public int? ContentId { get; set; }

    public string? Question { get; set; }

    public string? Option1 { get; set; }

    public string? Option2 { get; set; }

    public string? Option3 { get; set; }

    public string? Option4 { get; set; }

    public int? CorrectOptionNumber { get; set; }

    public int? Flag { get; set; }

    public virtual TbltopicContent? Content { get; set; }

    public virtual ICollection<TblstudentExamQuestion> TblstudentExamQuestions { get; set; } = new List<TblstudentExamQuestion>();
}
