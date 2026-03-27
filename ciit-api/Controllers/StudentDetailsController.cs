using ciit_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentDetailsController : BaseApiController
    {
        private readonly CiitstudContext _context;
        private readonly ILogger<StudentDetailsController> _logger;

        public StudentDetailsController(CiitstudContext context, ILogger<StudentDetailsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/students/details
        [HttpGet("details")]
        public async Task<IActionResult> GetAllStudentDetails()
        {
            try
            {
                var data = await _context.TblstudentDetails
                    .Where(s => s.Flag == 0)
                    .OrderBy(s => s.StudentId)
                    .AsNoTracking()
                    .Include(s => s.TblstudentQualifications.Where(q => q.Flag == 0))
                    .Include(s => s.TblstudentRegistrations.Where(r => r.Flag == 0))
                        .ThenInclude(r => r.TblstudentPayments.Where(p => p.Flag == 0))
                    .Include(s => s.TblstudentRegistrations.Where(r => r.Flag == 0))
                        .ThenInclude(r => r.Fee)
                            .ThenInclude(f => f!.Course)
                    .Select(s => new
                    {
                        s.StudentId,
                        s.StudentName,
                        s.LastName,
                        s.Gender,
                        s.MobileNumber,
                        s.WhatsappNumber,
                        s.EmailAddress,
                        s.BirthDate,
                        s.ProfilePhoto,
                        s.Qualification,
                        s.ParentName,
                        s.ParentNumber,
                        s.StudentCode,
                        s.LocalAddress,
                        s.PermanentAddress,
                        s.PermanentIdentificationNumber,
                        s.AadharCardNumber,
                        s.AadharCardPhoto,
                        s.BranchId,

                        Qualifications = s.TblstudentQualifications
                            .Select(q => new
                            {
                                q.QualificationId,
                                q.Qualification,
                                q.PassingYear,
                                q.University,
                                q.Medium,
                                q.Percentage
                            })
                            .ToList(),

                        Registrations = s.TblstudentRegistrations
                            .Select(r => new
                            {
                                r.RegistrationId,
                                r.StudentId,
                                r.RegistrationDate,
                                r.Discount,
                                r.CurrentStatus,

                                CourseFee = r.Fee == null ? null : new
                                {
                                    r.Fee.FeeId,
                                    r.Fee.CourseId,
                                    r.Fee.FeesAmount,
                                    r.Fee.Gst,
                                    r.Fee.FeeMode,
                                    r.Fee.FeesChangeDate,

                                    Course = r.Fee.Course == null ? null : new
                                    {
                                        r.Fee.Course.CourseId,
                                        r.Fee.Course.CourseName
                                    }
                                },

                                Payments = r.TblstudentPayments
                                    .Select(p => new
                                    {
                                        p.PaymentId,
                                        p.RegistrationId,
                                        p.PaymentDate,
                                        p.PaymentAmount,
                                        p.PaymentMode,
                                        p.PaymentDescription,
                                        p.IsPaid
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToListAsync();

                return ApiResponse(true, "Student details fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllStudentDetails");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

    }
}
