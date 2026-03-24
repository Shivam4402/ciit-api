using System;
using System.Collections.Generic;

namespace ciit_api.Models;

public partial class Tblenquiry
{
    public int EnquiryId { get; set; }

    public DateTime? EnquiryDate { get; set; }

    public string? CandidateName { get; set; }

    public string? Gender { get; set; }

    public string? LocalAddress { get; set; }

    public string? EmailAddress { get; set; }

    public string? MobileNumber { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? Qualification { get; set; }

    public string? LeadSources { get; set; }

    public string? EnquiryFors { get; set; }

    public string? InterestedTopics { get; set; }

    public string? Status { get; set; }

    public int? BranchId { get; set; }

    public virtual ICollection<TblenquiryFollowup> TblenquiryFollowups { get; set; } = new List<TblenquiryFollowup>();
}


public class CreateEnquiryDto
{
    public DateTime? EnquiryDate { get; set; }
    public string? CandidateName { get; set; }
    public string? Gender { get; set; }
    public string? LocalAddress { get; set; }
    public string? EmailAddress { get; set; }
    public string? MobileNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Qualification { get; set; }
    public string? LeadSources { get; set; }
    public string? EnquiryFors { get; set; }
    public string? InterestedTopics { get; set; }
    //public string? Status { get; set; }
    public int? BranchId { get; set; }
}

public class UpdateEnquiryDto : CreateEnquiryDto
{
}

public class EnquiryResponseDto
{
    public int EnquiryId { get; set; }
    public DateTime? EnquiryDate { get; set; }
    public string? CandidateName { get; set; }
    public string? Gender { get; set; }
    public string? EmailAddress { get; set; }
    public DateTime? BirthDate { get; set; }    
    public string? Qualification { get; set; }
    public string? MobileNumber { get; set; }
    public string? LeadSources { get; set; }
    public string? EnquiryFors { get; set; }
    public string? InterestedTopics { get; set; }
    public string? Status { get; set; }
    public int? BranchId { get; set; }
}