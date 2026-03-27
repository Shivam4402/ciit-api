using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class Tblqualification
{
    public int QualificationId { get; set; }

    public string Qualification { get; set; } = null!;

    public int? Flag { get; set; }
}
