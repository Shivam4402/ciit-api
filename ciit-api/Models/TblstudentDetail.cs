using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class TblstudentDetail
{
    public int StudentId { get; set; }

    public string? StudentName { get; set; }

    public string? Gender { get; set; }

    public string? MobileNumber { get; set; }

    public string EmailAddress { get; set; } = null!;

    public string? Password { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? ProfilePhoto { get; set; }

    public string? Qualification { get; set; }

    public int? Flag { get; set; }

    public string? ParentName { get; set; }

    public string? ParentNumber { get; set; }

    public string? StudentCode { get; set; }

    public string? LastName { get; set; }

    public string? WhatsappNumber { get; set; }

    public string? LocalAddress { get; set; }

    public string? PermanentAddress { get; set; }

    public string PermanentIdentificationNumber { get; set; } = null!;

    public string? AadharCardNumber { get; set; }

    public string? AadharCardPhoto { get; set; }

    public int? BranchId { get; set; }

    public virtual Tblbranch? Branch { get; set; }

    public virtual ICollection<TblstudentQualification> TblstudentQualifications { get; set; } = new List<TblstudentQualification>();

    public virtual ICollection<TblstudentRegistration> TblstudentRegistrations { get; set; } = new List<TblstudentRegistration>();
}
