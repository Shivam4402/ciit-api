using ciit_api.Models;
using ciit_api.Services.Implementations;
using ciit_api.Services.Interfaces;
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
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentDetailsController> _logger;

        public StudentDetailsController(CiitstudContext context, IStudentService studentService, ILogger<StudentDetailsController> logger)
        {
            _context = context;
            _studentService = studentService;
            _logger = logger;
        }

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


        [HttpGet("details/{studentId}")]
        public async Task<IActionResult> GetStudentDetailsById(int studentId)
        {
            try
            {
                if (studentId <= 0)
                    return ApiResponse(false, "Invalid student id", statusCode: 400);

                var data = await _context.TblstudentDetails
                    .Where(s => s.Flag == 0 && s.StudentId == studentId)
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
                    .FirstOrDefaultAsync();

                if (data == null)
                    return ApiResponse(false, "Student not found", statusCode: 404);

                return ApiResponse(true, "Student details fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetStudentDetailsById");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }


        [HttpGet("student-wise-batches/{studentId}")]
        public async Task<IActionResult> GetStudentWiseBatchDetails(int studentId)
        {
            try
            {
                if (studentId <= 0)
                    return ApiResponse(false, "Invalid student id", statusCode: 400);

                var result = await _studentService.GetStudentWiseBatchDetails(studentId);

                if (result == null || result.Count == 0)
                    return ApiResponse(false, "No batch found for this student", data: result, statusCode: 404);

                return ApiResponse(true, "Student batch details fetched successfully", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetStudentWiseBatchDetails");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }

        }


        [HttpGet("student-batch-attendance/{batchId}/{registrationId}")]
        public async Task<IActionResult> GetStudentBatchAttendance(int batchId, int registrationId)
        {
            try
            {
                if (batchId <= 0 || registrationId <= 0)
                    return ApiResponse(false, "Invalid batchId or registrationId", statusCode: 400);

                var result = await _studentService.GetStudentBatchWiseAttendance(batchId, registrationId);

                if (result == null || result.Count == 0)
                    return ApiResponse(false, "No attendance found", data: result, statusCode: 404);

                return ApiResponse(true, "Student attendance fetched successfully", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetStudentBatchAttendance");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }
    }
}
