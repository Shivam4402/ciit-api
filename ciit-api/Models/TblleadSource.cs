using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ciit_api.Models;

public partial class TblleadSource
{
    public int SourceId { get; set; }

    public string? SourceName { get; set; }

    public int? Flag { get; set; }
}

public class CreateLeadSourceDto
{
    [Required]
    public string SourceName { get; set; } = null!;
}

public class UpdateLeadSourceDto
{
    [Required]
    public string SourceName { get; set; } = null!;
}

public class LeadSourceResponseDto
{
    public int SourceId { get; set; }

    public string? SourceName { get; set; }
}