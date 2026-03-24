using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class Tblbranch
{
    public int BranchId { get; set; }

    public string BranchName { get; set; } = null!;
}

public class CreateBranchDto
{
    public string BranchName { get; set; } = null!;
}

public class UpdateBranchDto : CreateBranchDto
{
}

public class BranchResponseDto
{
    public int BranchId { get; set; }
    public string BranchName { get; set; } = null!;
}