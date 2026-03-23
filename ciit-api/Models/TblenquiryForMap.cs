using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class TblenquiryForMap
{
    public int Id { get; set; }

    public int? EnquiryId { get; set; }

    public int? EnquiryForId { get; set; }
}
