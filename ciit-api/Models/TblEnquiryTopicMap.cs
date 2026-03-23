using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class TblEnquiryTopicMap
{
    public int Id { get; set; }

    public int? EnquiryId { get; set; }

    public int? TopicId { get; set; }
}
