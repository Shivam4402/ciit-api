using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class TblenquiryFollowup
{
    public int FollowupId { get; set; }

    public int? EnquiryId { get; set; }

    public DateTime? FollowUpDate { get; set; }

    public string? FollowUpBy { get; set; }

    public string? Description { get; set; }

    public virtual Tblenquiry? Enquiry { get; set; }
}
