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
        private readonly IWebHostEnvironment _env;


        public StudentDetailsController(CiitstudContext context, IStudentService studentService, ILogger<StudentDetailsController> logger, IWebHostEnvironment env)
        {
            _context = context;
            _studentService = studentService;
            _logger = logger;
            _env = env;
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


        [HttpGet("student-batch-exams/{registrationId}")]
        public async Task<IActionResult> GetStudentWiseBatchExams(int registrationId)
        {
            try
            {
                if (registrationId <= 0)
                    return ApiResponse(false, "Invalid registration id", statusCode: 400);

                var result = await _studentService.GetStudentWiseBatchExams(registrationId);

                if (result == null || result.Count == 0)
                    return ApiResponse(false, "No exams found for this student", data: result, statusCode: 404);

                return ApiResponse(true, "Student batch exams fetched successfully", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetStudentWiseBatchExams");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }


        [HttpGet("student-course-topics/{registrationId}")]
        public async Task<IActionResult> GetStudentWiseCourseTopics(int registrationId)
        {
            try
            {
                if (registrationId <= 0)
                    return ApiResponse(false, "Invalid registration id", statusCode: 400);

                var registration = await _context.TblstudentRegistrations
                    .AsNoTracking()
                    .Where(r => r.RegistrationId == registrationId
                                && r.Flag == 0
                                && r.Student != null
                                && r.Student.Flag == 0)
                    .Select(r => new
                    {
                        r.RegistrationId,
                        r.StudentId,
                        StudentName = r.Student!.StudentName,
                        CourseId = r.Fee != null ? r.Fee.CourseId : null,
                        CourseName = r.Fee != null && r.Fee.Course != null ? r.Fee.Course.CourseName : null
                    })
                    .FirstOrDefaultAsync();

                if (registration == null)
                    return ApiResponse(false, "Registration not found", statusCode: 404);

                if (registration.CourseId == null)
                    return ApiResponse(false, "No course mapped with this registration", statusCode: 404);

                var topics = await _context.TbltrainingCourseTopics
                    .AsNoTracking()
                    .Where(ct => ct.CourseId == registration.CourseId
                                 && ct.Flag == 0
                                 && ct.Topic != null
                                 && ct.Topic.Flag == 0)
                    .OrderBy(ct => ct.CourseTopicId)
                    .Select(ct => new
                    {
                        ct.CourseTopicId,
                        ct.CourseId,
                        ct.TopicId,
                        TopicName = ct.Topic!.TopicName,
                        PublicFolderId = ct.Topic.Publicfolderid
                    })
                    .ToListAsync();

                return ApiResponse(true, "Registration wise course topics fetched successfully", new
                {
                    registration.RegistrationId,
                    registration.StudentId,
                    registration.StudentName,
                    registration.CourseId,
                    registration.CourseName,
                    Topics = topics
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetStudentWiseCourseTopics {RegistrationId}", registrationId);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }



        [HttpPost("student-change-profile-photo-upload/{studentId:int}")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> StudentChangeProfilePhotoUpload(int studentId, IFormFile profilePhoto)
        {
            try
            {
                if (studentId <= 0)
                    return ApiResponse(false, "Invalid student id", statusCode: 400);

                if (profilePhoto == null || profilePhoto.Length == 0)
                    return ApiResponse(false, "ProfilePhoto is required", statusCode: 400);

                var ext = Path.GetExtension(profilePhoto.FileName).ToLowerInvariant();
                var allowedExt = new[] { ".jpg", ".jpeg", ".png", ".webp" };

                if (!allowedExt.Contains(ext))
                    return ApiResponse(false, "Only .jpg, .jpeg, .png, .webp files are allowed", statusCode: 400);

                var student = await _context.TblstudentDetails
                    .FirstOrDefaultAsync(s => s.Flag == 0 && s.StudentId == studentId);

                if (student == null)
                    return ApiResponse(false, "Student not found", statusCode: 404);

                var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var uploadsRoot = Path.Combine(webRoot, "uploads", "students");
                Directory.CreateDirectory(uploadsRoot);

                var fileName = $"student_{student.StudentId}_{DateTime.UtcNow:yyyyMMddHHmmssfff}{ext}";
                var filePath = Path.Combine(uploadsRoot, fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePhoto.CopyToAsync(stream);
                }


                if (!string.IsNullOrEmpty(student.ProfilePhoto))
                {
                    try
                    {
                        var oldImageUrl = student.ProfilePhoto;

                        if (Uri.TryCreate(oldImageUrl, UriKind.Absolute, out var uri))
                        {
                            var relativePath = uri.AbsolutePath.TrimStart('/');
                            var oldFilePath = Path.Combine(webRoot, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to delete old profile photo for StudentId {StudentId}", studentId);
                    }
                }

                var request = HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";

                student.ProfilePhoto = $"{baseUrl}/uploads/students/{fileName}";
                await _context.SaveChangesAsync();

                return ApiResponse(true, "Profile photo updated successfully", new
                {
                    student.StudentId,
                    student.ProfilePhoto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in StudentChangeProfilePhotoUpload {StudentId}", studentId);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }
    }
}
    