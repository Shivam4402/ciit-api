using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class TblstudentQualification
{
    public int QualificationId { get; set; }

    public int? StudentId { get; set; }

    public string? Qualification { get; set; }

    public int? PassingYear { get; set; }

    public string? University { get; set; }

    public string? Medium { get; set; }

    public double? Percentage { get; set; }

    public int? Flag { get; set; }

    public virtual TblstudentDetail? Student { get; set; }
}
