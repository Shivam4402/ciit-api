using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class TblleadSource
{
    public int SourceId { get; set; }

    public string? SourceName { get; set; }

    public int? Flag { get; set; }
}
