using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class TbltopicContent
{
    public int ContentId { get; set; }

    public int? TopicId { get; set; }

    public string? ContentName { get; set; }

    public int? Flag { get; set; }

    public virtual ICollection<TblcontentQuestion> TblcontentQuestions { get; set; } = new List<TblcontentQuestion>();

    public virtual TbltrainingTopic? Topic { get; set; }
}
