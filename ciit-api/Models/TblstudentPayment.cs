using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class TblstudentPayment
{
    public int PaymentId { get; set; }

    public int? RegistrationId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public double? PaymentAmount { get; set; }

    public string? PaymentMode { get; set; }

    public string? PaymentDescription { get; set; }

    public int? Flag { get; set; }

    public int? IsPaid { get; set; }

    public virtual TblstudentRegistration? Registration { get; set; }
}
