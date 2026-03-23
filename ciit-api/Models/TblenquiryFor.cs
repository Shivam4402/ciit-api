using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ciit_api.Models;

public partial class TblenquiryFor
{
    public int EnquiryForId { get; set; }

    public string? EnquiryFor { get; set; }

    public int? Flag { get; set; }
}

public class CreateEnquiryForDto
{
    [Required]
    public string EnquiryFor { get; set; } = null!;
}

public class UpdateEnquiryForDto
{
    [Required]
    public string EnquiryFor { get; set; } = null!;
}

public class EnquiryForResponseDto
{
    public int EnquiryForId { get; set; }

    public string? EnquiryFor { get; set; }
}