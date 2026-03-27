using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class Tblbranch
{
    public int BranchId { get; set; }

    public string BranchName { get; set; } = null!;

    public virtual ICollection<Tblenquiry> Tblenquiries { get; set; } = new List<Tblenquiry>();

    public virtual ICollection<TblstudentDetail> TblstudentDetails { get; set; } = new List<TblstudentDetail>();
}
