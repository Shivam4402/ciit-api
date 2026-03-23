using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ciit_api.Models;

public partial class TbltrainingCourseFee
{
    public int FeeId { get; set; }

    public int? CourseId { get; set; }

    public double FeesAmount { get; set; }

    public double? Gst { get; set; }

    public string? FeeMode { get; set; }

    public int? Flag { get; set; }

    public DateOnly? FeesChangeDate { get; set; }

    public virtual TbltrainingCourse? Course { get; set; }
}

public class CreateCourseFeeDto
{
    [Required]
    public int CourseId { get; set; }

    [Required]
    public double FeesAmount { get; set; }

    public double? Gst { get; set; }

    public string? FeeMode { get; set; }

}

public class UpdateCourseFeeDto
{
    [Required]
    public double FeesAmount { get; set; }

    public double? Gst { get; set; }

    public string? FeeMode { get; set; }

}   

public class CourseFeeResponseDto
{
    public int FeeId { get; set; }

    public int? CourseId { get; set; }

    public double FeesAmount { get; set; }

    public double? Gst { get; set; }

    public string? FeeMode { get; set; }

    public DateOnly? FeesChangeDate { get; set; }
}