using ciit_api.DTOs.Enquiry;
using ciit_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Controllers
{
    [Route("api/enquiries")]
    [ApiController]
    public class EnquiryController : BaseApiController
    {
        private readonly CiitstudContext _context;
        private readonly ILogger<EnquiryController> _logger;

        public EnquiryController(CiitstudContext context, ILogger<EnquiryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEnquiries()
        {
            try
            {
                var data = await _context.Tblenquiries
                    .OrderBy(x => x.EnquiryId)
                    .Select(x => new EnquiryResponseDto
                    {
                        EnquiryId = x.EnquiryId,
                        EnquiryDate = x.EnquiryDate,
                        CandidateName = x.CandidateName,
                        Gender = x.Gender,
                        EmailAddress = x.EmailAddress,
                        MobileNumber = x.MobileNumber,
                        Qualification = x.Qualification,
                        BirthDate = x.BirthDate,
                        LeadSources = x.LeadSources,
                        EnquiryFors = x.EnquiryFors,
                        InterestedTopics = x.InterestedTopics,
                        Status = x.Status,
                        BranchId = x.BranchId,
                        BranchName = x.Branch.BranchName
                    })
                    .ToListAsync();

                return ApiResponse(true, "Enquiries fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllEnquiries");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnquiryById(int id)
        {
            try
            {
                var data = await _context.Tblenquiries
                    .Where(x => x.EnquiryId == id)
                    .Select(x => new EnquiryResponseDto
                    {
                        EnquiryId = x.EnquiryId,
                        EnquiryDate = x.EnquiryDate,
                        CandidateName = x.CandidateName,
                        Gender = x.Gender,
                        EmailAddress = x.EmailAddress,
                        MobileNumber = x.MobileNumber,
                        Qualification = x.Qualification,
                        BirthDate = x.BirthDate,
                        LeadSources = x.LeadSources,
                        EnquiryFors = x.EnquiryFors,
                        InterestedTopics = x.InterestedTopics,
                        Status = x.Status,
                        BranchId = x.BranchId
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                    return ApiResponse(false, $"Enquiry with ID {id} not found", statusCode: 404);

                return ApiResponse(true, "Enquiry fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetEnquiryById {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

            //[Authorize]
            [HttpPost]
            public async Task<IActionResult> CreateEnquiry([FromBody] CreateEnquiryDto dto)
            {
                if (!ModelState.IsValid)
                {
                    return ApiResponse(false, "Validation failed",
                        errors: ModelState.Values.SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage).ToList(),
                        statusCode: 400);
                }

                try
                {
                    var entity = new Tblenquiry
                    {
                        EnquiryDate = dto.EnquiryDate ?? DateTime.UtcNow,
                        CandidateName = dto.CandidateName,
                        Gender = dto.Gender,
                        LocalAddress = dto.LocalAddress,
                        EmailAddress = dto.EmailAddress,
                        MobileNumber = dto.MobileNumber,
                        BirthDate = dto.BirthDate,
                        Qualification = dto.Qualification,
                        LeadSources = dto.LeadSources,
                        EnquiryFors = dto.EnquiryFors,
                        InterestedTopics = dto.InterestedTopics,
                        Status = "Enquiry Submitted",
                        BranchId = dto.BranchId
                    };

                    _context.Tblenquiries.Add(entity);
                    await _context.SaveChangesAsync();

                    return ApiResponse(true, "Enquiry created successfully", new
                    {
                        entity.EnquiryId,
                        entity.CandidateName
                    }, statusCode: 201);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in CreateEnquiry");
                    return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
                }
            }

        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnquiry(int id, [FromBody] UpdateEnquiryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(false, "Validation failed",
                    errors: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(),
                    statusCode: 400);
            }

            try
            {
                var entity = await _context.Tblenquiries
                    .FirstOrDefaultAsync(x => x.EnquiryId == id);

                if (entity == null)
                    return ApiResponse(false, $"Enquiry with ID {id} not found", statusCode: 404);

                entity.EnquiryDate = dto.EnquiryDate;
                entity.CandidateName = dto.CandidateName;
                entity.Gender = dto.Gender;
                entity.LocalAddress = dto.LocalAddress;
                entity.EmailAddress = dto.EmailAddress;
                entity.MobileNumber = dto.MobileNumber;
                entity.BirthDate = dto.BirthDate;
                entity.Qualification = dto.Qualification;
                entity.LeadSources = dto.LeadSources;
                entity.EnquiryFors = dto.EnquiryFors;
                entity.InterestedTopics = dto.InterestedTopics;
                entity.Status = "Enquiry Updated";
                entity.BranchId = dto.BranchId;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Enquiry updated successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateEnquiry {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        // DELETE (Hard Delete)

        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnquiry(int id)
        {
            try
            {
                var entity = await _context.Tblenquiries
                    .FirstOrDefaultAsync(x => x.EnquiryId == id);

                if (entity == null)
                    return ApiResponse(false, $"Enquiry with ID {id} not found", statusCode: 404);

                _context.Tblenquiries.Remove(entity);
                await _context.SaveChangesAsync();

                return ApiResponse(true, "Enquiry deleted successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteEnquiry {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }
    }
}
