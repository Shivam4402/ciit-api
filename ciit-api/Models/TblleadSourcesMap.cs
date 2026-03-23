using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class TblleadSourcesMap
{
    public int Id { get; set; }

    public int? EnquiryId { get; set; }

    public int? SourceId { get; set; }
}
