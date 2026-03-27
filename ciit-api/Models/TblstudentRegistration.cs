using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class TblstudentRegistration
{
    public int RegistrationId { get; set; }

    public int? StudentId { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public double? Discount { get; set; }

    public int? FeeId { get; set; }

    public int? Flag { get; set; }

    public string? CurrentStatus { get; set; }

    public virtual TbltrainingCourseFee? Fee { get; set; }

    public virtual TblstudentDetail? Student { get; set; }

    public virtual ICollection<TblstudentPayment> TblstudentPayments { get; set; } = new List<TblstudentPayment>();
}
