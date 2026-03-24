using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class Tblqualification
{
    public int QualificationId { get; set; }

    public string Qualification { get; set; } = null!;

    public int? Flag { get; set; }
}

public class CreateQualificationDto
{
    public string Qualification { get; set; } = null!;
}

public class UpdateQualificationDto : CreateQualificationDto
{
}

public class QualificationResponseDto
{
    public int QualificationId { get; set; }
    public string Qualification { get; set; } = null!;
}
