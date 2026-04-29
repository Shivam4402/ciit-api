using ciit_api.DTOs.Student;
using ciit_api.Models;
using ciit_api.Services.Implementations;
using ciit_api.Services.Interfaces;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ciit_api.Controllers
{
    //[Authorize]
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

                var ext = System.IO.Path.GetExtension(profilePhoto.FileName).ToLowerInvariant();
                var allowedExt = new[] { ".jpg", ".jpeg", ".png", ".webp" };

                if (!allowedExt.Contains(ext))
                    return ApiResponse(false, "Only .jpg, .jpeg, .png, .webp files are allowed", statusCode: 400);

                var student = await _context.TblstudentDetails
                    .FirstOrDefaultAsync(s => s.Flag == 0 && s.StudentId == studentId);

                if (student == null)
                    return ApiResponse(false, "Student not found", statusCode: 404);

                var webRoot = _env.WebRootPath ?? System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot");
                var uploadsRoot = System.IO.Path.Combine(webRoot, "uploads", "students");
                System.IO.Directory.CreateDirectory(uploadsRoot);

                var fileName = $"student_{student.StudentId}_{DateTime.UtcNow:yyyyMMddHHmmssfff}{ext}";
                var filePath = System.IO.Path.Combine(uploadsRoot, fileName);

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
                            var oldFilePath = System.IO.Path.Combine(webRoot, relativePath.Replace("/", System.IO.Path.DirectorySeparatorChar.ToString()));

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



        [HttpGet("student-exam-report/{registrationId:int}")]
        public async Task<IActionResult> GetStudentExamReport(int registrationId)
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
                        StudentName = r.Student!.StudentName
                    })
                    .FirstOrDefaultAsync();

                if (registration == null)
                    return ApiResponse(false, "Registration not found", statusCode: 404);

                var examData = await _context.TblstudentExams
                    .AsNoTracking()
                    .Where(e => e.StudentId == registration.StudentId && e.Status== "Submitted")
                    .Select(e => new
                    {
                        e.ExamId,
                        e.ExamDate,
                        e.StartTime,
                        e.EndTime,
                        e.TotalQuestions,
                        TopicName = e.Topic != null ? e.Topic.TopicName : null,
                        CorrectQuestions = e.TblstudentExamQuestions.Count(eq =>
                            eq.SubmittedOptionNumber != null
                            && eq.Question != null
                            && eq.Question.CorrectOptionNumber != null
                            && eq.SubmittedOptionNumber == eq.Question.CorrectOptionNumber),
                        AttemptedQuestions = e.TblstudentExamQuestions.Count(eq =>
                            eq.SubmittedOptionNumber != null
                            && eq.Question != null
                            && eq.Question.CorrectOptionNumber != null)
                    })
                    .OrderBy(e => e.ExamDate)
                    .ToListAsync();

                var result = examData.Select(e =>
                {
                    var total = e.TotalQuestions ?? e.AttemptedQuestions;
                    var percentage = total == 0
                        ? 0m
                        : Math.Round((decimal)e.CorrectQuestions * 100m / total, 2);

                    return new StudentExamReportDto
                    {
                        ExamId = e.ExamId,
                        RegistrationId = registration.RegistrationId,
                        StudentName = registration.StudentName,
                        TopicName = e.TopicName,
                        ExamDate = e.ExamDate,
                        StartTime = e.StartTime?.ToString("h:mm:ss tt"),
                        EndTime = e.EndTime?.ToString("h:mm:ss tt"),
                        TotalQuestions = total,
                        CorrectQuestions = e.CorrectQuestions,
                        WrongQuestions = Math.Max(0, total - e.CorrectQuestions),
                        Percentage = percentage,
                        Grade = GetGrade(percentage)
                    };
                }).ToList();

                if (result.Count == 0)
                    return ApiResponse(false, "No exams found for this registration", data: result, statusCode: 404);

                return ApiResponse(true, "Student exam report fetched successfully", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetStudentExamReport {RegistrationId}", registrationId);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        private static string GetGrade(decimal percentage)
        {
            return percentage switch
            {
                >= 80 => "Excellent",
                >= 60 => "Good",
                >= 40 => "Average",
                _ => "Poor"
            };
        }




        //[HttpPost("generate-exam-report-certificate")]
        //public async Task<IActionResult> GenerateExamReportCertificate([FromBody] StudentExamReportCertificateRequestDto dto)
        //{
        //    try
        //    {
        //        if (dto == null || dto.RegistrationId <= 0)
        //            return ApiResponse(false, "Invalid registration id", statusCode: 400);

        //        var registration = await _context.TblstudentRegistrations
        //            .AsNoTracking()
        //            .Where(r => r.RegistrationId == dto.RegistrationId
        //                        && r.Flag == 0
        //                        && r.Student != null
        //                        && r.Student.Flag == 0)
        //            .Select(r => new
        //            {
        //                r.RegistrationId,
        //                r.StudentId,
        //                FirstName = r.Student!.StudentName,
        //                LastName = r.Student.LastName
        //            })
        //            .FirstOrDefaultAsync();

        //        if (registration == null)
        //            return ApiResponse(false, "Registration not found", statusCode: 404);

        //        var examData = await _context.TblstudentExams
        //            .AsNoTracking()
        //            .Where(e => e.StudentId == registration.StudentId && e.Status == "Submitted")
        //            .Select(e => new
        //            {
        //                e.ExamId,
        //                e.ExamDate,
        //                e.StartTime,
        //                e.EndTime,
        //                e.TotalQuestions,
        //                TopicName = e.Topic != null ? e.Topic.TopicName : null,
        //                CorrectQuestions = e.TblstudentExamQuestions.Count(eq =>
        //                    eq.SubmittedOptionNumber != null
        //                    && eq.Question != null
        //                    && eq.Question.CorrectOptionNumber != null
        //                    && eq.SubmittedOptionNumber == eq.Question.CorrectOptionNumber),
        //                AttemptedQuestions = e.TblstudentExamQuestions.Count(eq =>
        //                    eq.SubmittedOptionNumber != null
        //                    && eq.Question != null
        //                    && eq.Question.CorrectOptionNumber != null)
        //            })
        //            .OrderBy(e => e.ExamDate)
        //            .ToListAsync();

        //        if (examData.Count == 0)
        //            return ApiResponse(false, "No exams found for this registration", statusCode: 404);

        //        var fullName = $"{registration.FirstName} {registration.LastName}".Trim();
        //        var latest = examData.Last();

        //        var latestTotal = latest.TotalQuestions ?? latest.AttemptedQuestions;
        //        var latestPercentage = latestTotal == 0
        //            ? 0m
        //            : Math.Round((decimal)latest.CorrectQuestions * 100m / latestTotal, 2);
        //        using var stream = new MemoryStream();
        //        var writer = new PdfWriter(stream);
        //        var pdf = new PdfDocument(writer);
        //        var document = new Document(pdf, PageSize.A4);

        //        document.SetMargins(50, 50, 50, 50);

        //        // 🎨 COLORS
        //        var primary = new DeviceRgb(13, 71, 161);     // Royal Blue
        //        var gold = new DeviceRgb(212, 175, 55);       // Gold

        //        // 🔤 FONTS
        //        var fontRegular = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        //        var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

        //        // 📄 PAGE
        //        var page = pdf.AddNewPage();
        //        var pageSize = page.GetPageSize();
        //        var canvas = new PdfCanvas(page);

        //        // ==============================
        //        // 🟦 BORDER (DOUBLE STYLE)
        //        // ==============================
        //        canvas.SetLineWidth(2f);
        //        canvas.SetStrokeColor(gold);
        //        canvas.Rectangle(20, 20, pageSize.GetWidth() - 40, pageSize.GetHeight() - 40);
        //        canvas.Stroke();

        //        canvas.SetLineWidth(0.5f);
        //        canvas.Rectangle(30, 30, pageSize.GetWidth() - 60, pageSize.GetHeight() - 60);
        //        canvas.Stroke();

        //        // ==============================
        //        // 💧 WATERMARK LOGO (LIGHT)
        //        // ==============================
        //        var logoPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "mainlogo.png");

        //        if (System.IO.File.Exists(logoPath)) // ✅ FIXED
        //        {
        //            var img = new Image(ImageDataFactory.Create(logoPath))
        //                .ScaleToFit(300, 300)
        //                .SetFixedPosition(
        //                    pageSize.GetWidth() / 2 - 150,
        //                    pageSize.GetHeight() / 2 - 150
        //                );

        //            var pdfCanvas = new PdfCanvas(page);

        //            pdfCanvas.SaveState();
        //            pdfCanvas.SetExtGState(new PdfExtGState().SetFillOpacity(0.08f));

        //            var canvasModel = new Canvas(pdfCanvas, pageSize); // ✅ FIXED
        //            canvasModel.Add(img);

        //            pdfCanvas.RestoreState();
        //        }

        //        // ==============================
        //        // 🏢 HEADER
        //        // ==============================
        //        document.Add(new Paragraph("CIIT TRAINING INSTITUTE")
        //            .SetTextAlignment(TextAlignment.CENTER)
        //            .SetFont(fontBold)
        //            .SetFontSize(16)
        //            .SetFontColor(primary));

        //        document.Add(new Paragraph("CERTIFICATE OF EXAM")
        //            .SetTextAlignment(TextAlignment.CENTER)
        //            .SetFont(fontBold)
        //            .SetFontSize(26)
        //            .SetMarginTop(10));

        //        // ==============================
        //        // 📜 BODY TEXT
        //        // ==============================
        //        document.Add(new Paragraph("This is to certify that")
        //            .SetTextAlignment(TextAlignment.CENTER)
        //            .SetFont(fontRegular)
        //            .SetFontSize(12)
        //            .SetMarginTop(20));

        //        // 👤 NAME (MAIN HIGHLIGHT)
        //        document.Add(new Paragraph(fullName.ToUpper())
        //            .SetTextAlignment(TextAlignment.CENTER)
        //            .SetFont(fontBold)
        //            .SetFontSize(28)
        //            .SetMarginTop(10)
        //            .SetCharacterSpacing(2));

        //        // DESCRIPTION
        //        document.Add(new Paragraph("has successfully completed the examination in")
        //            .SetTextAlignment(TextAlignment.CENTER)
        //            .SetFontSize(12)
        //            .SetMarginTop(15));

        //        // 📘 COURSE NAME
        //        document.Add(new Paragraph(latest.TopicName ?? "-")
        //            .SetTextAlignment(TextAlignment.CENTER)
        //            .SetFont(fontBold)
        //            .SetFontSize(18)
        //            .SetFontColor(primary)
        //            .SetMarginTop(5));

        //        // 🏆 RESULT
        //        document.Add(new Paragraph($"Grade: {GetGrade(latestPercentage)}  ({latestPercentage}%)")
        //            .SetTextAlignment(TextAlignment.CENTER)
        //            .SetFont(fontBold)
        //            .SetFontSize(14)
        //            .SetMarginTop(15));

        //        // 📅 DATE
        //        document.Add(new Paragraph($"Date: {latest.ExamDate?.ToString("dd MMMM yyyy")}")
        //            .SetTextAlignment(TextAlignment.CENTER)
        //            .SetFontSize(11)
        //            .SetMarginTop(10));

        //        // ==============================
        //        // ✍️ SIGNATURE SECTION
        //        // ==============================
        //        var table = new Table(3).UseAllAvailableWidth().SetMarginTop(50);

        //        table.AddCell(GetSignatureCell("Instructor", fontRegular));
        //        table.AddCell(GetSignatureCell("Seal", fontRegular));
        //        table.AddCell(GetSignatureCell("Director", fontRegular));

        //        document.Add(table);

        //        // ==============================
        //        // 🆔 CERTIFICATE ID
        //        // ==============================
        //        document.Add(new Paragraph($"Certificate ID: CIIT-{registration.RegistrationId}")
        //            .SetTextAlignment(TextAlignment.RIGHT)
        //            .SetFontSize(8)
        //            .SetMarginTop(20));

        //        // ==============================
        //        // CLOSE
        //        // ==============================
        //        document.Close();

        //        var base64 = Convert.ToBase64String(stream.ToArray());
        //        return Ok(base64);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error in GenerateExamReportCertificate {RegistrationId}", dto?.RegistrationId);
        //        return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
        //    }
        //}

        //private Cell GetSignatureCell(string title, PdfFont font)
        //{
        //    return new Cell()
        //        .Add(new Paragraph("\n\n\n----------------------"))
        //        .Add(new Paragraph(title).SetFont(font).SetFontSize(10))
        //        .SetTextAlignment(TextAlignment.CENTER)
        //        .SetBorder(Border.NO_BORDER);
        //}


        [HttpPost("generate-exam-report-certificate")]
        public async Task<IActionResult> GenerateExamReportCertificate([FromBody] StudentExamReportCertificateRequestDto dto)
        {
            try
            {
                if (dto == null || dto.RegistrationId <= 0)
                    return ApiResponse(false, "Invalid registration id", statusCode: 400);

                var registration = await _context.TblstudentRegistrations
                    .AsNoTracking()
                    .Where(r => r.RegistrationId == dto.RegistrationId
                                && r.Flag == 0
                                && r.Student != null
                                && r.Student.Flag == 0)
                    .Select(r => new
                    {
                        r.RegistrationId,
                        r.StudentId,
                        FirstName = r.Student!.StudentName,
                        LastName = r.Student.LastName,
                        Pin = r.Student.PermanentIdentificationNumber
                    })
                    .FirstOrDefaultAsync();

                if (registration == null)
                    return ApiResponse(false, "Registration not found", statusCode: 404);

                var examData = await _context.TblstudentExams
                    .AsNoTracking()
                    .Where(e => e.StudentId == registration.StudentId && e.Status == "Submitted")
                    .Select(e => new
                    {
                        e.ExamId,
                        e.ExamDate,
                        e.StartTime,
                        e.EndTime,
                        e.TotalQuestions,
                        TopicName = e.Topic != null ? e.Topic.TopicName : null,
                        CorrectQuestions = e.TblstudentExamQuestions.Count(eq =>
                            eq.SubmittedOptionNumber != null
                            && eq.Question != null
                            && eq.Question.CorrectOptionNumber != null
                            && eq.SubmittedOptionNumber == eq.Question.CorrectOptionNumber),
                        AttemptedQuestions = e.TblstudentExamQuestions.Count(eq =>
                            eq.SubmittedOptionNumber != null
                            && eq.Question != null
                            && eq.Question.CorrectOptionNumber != null)
                    })
                    .OrderBy(e => e.ExamDate)
                    .ToListAsync();

                if (examData.Count == 0)
                    return ApiResponse(false, "No exams found for this registration", statusCode: 404);

                var fullName = $"{registration.FirstName} {registration.LastName}".Trim();
                var latest = examData.Last();
                var latestTotal = latest.TotalQuestions ?? latest.AttemptedQuestions;
                var latestPercentage = latestTotal == 0
                    ? 0m
                    : Math.Round((decimal)latest.CorrectQuestions * 100m / latestTotal, 2);

                using var stream = new MemoryStream();
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf, PageSize.A4);

                document.SetMargins(40, 40, 40, 40);

                var page = pdf.AddNewPage();
                var pageSize = page.GetPageSize();
                var canvas = new PdfCanvas(page);
                var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);


                // ================= BORDER =================
                canvas.SetLineWidth(1.5f);
                canvas.RoundRectangle(10, 10, pageSize.GetWidth() - 20, pageSize.GetHeight() - 20, 20);
                canvas.Stroke();


                // Logo top-left
                // ================= LOGO (TOP LEFT) =================
                var logoPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "mainlogo.png");
                if (System.IO.File.Exists(logoPath))
                {
                    var logo = new Image(ImageDataFactory.Create(logoPath))
                        .ScaleToFit(80, 80)
                        .SetFixedPosition(25, pageSize.GetHeight() - 90);

                    document.Add(logo);
                }

                // ================= TITLE =================
                document.Add(new Paragraph("Certificate")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(16)
                    .SetMarginTop(20));

                // ================= BODY =================
                document.Add(new Paragraph("\n"));

                document.Add(new Paragraph("The Academic Council of CIIT")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12)
                    .SetMarginTop(10));

                document.Add(new Paragraph("Certifies that")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12)
                    .SetMarginTop(5));

                document.Add(new Paragraph("\n"));

                // ================= NAME =================
                document.Add(new Paragraph(fullName)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(18)
                    .SetFont(boldFont)
                    .SetMarginTop(15));

                // ================= DETAILS =================
                document.Add(new Paragraph("has completed a exam on")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(10)
                    .SetMarginTop(5));

                document.Add(new Paragraph(latest.TopicName ?? "-")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(14)
                    .SetFont(boldFont)
                    .SetMarginTop(5));

                document.Add(new Paragraph("with grade")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(10)
                    .SetMarginTop(5));

                document.Add(new Paragraph(GetGrade(latestPercentage))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(14)
                    .SetFont(boldFont)
                    .SetMarginTop(5));

                // ================= SIGNATURE =================
                document.Add(new Paragraph("\n\n"));

                document.Add(new Paragraph("Director")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(9));

                document.Add(new Paragraph("CIIT Training Institute Pvt Ltd")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(9));

                // ================= DIVIDER =================
                document.Add(new LineSeparator(new SolidLine()).SetMarginTop(10));

                document.Add(new Paragraph($"PIN: {registration.Pin ?? "-"}        Certificate No: CIIT{registration.RegistrationId}")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(9));

                document.Add(new LineSeparator(new SolidLine()).SetMarginBottom(5));

                // Score details table (keep score details)
                // ================= TABLE =================
                var scoreTable = new Table(10).UseAllAvailableWidth().SetMarginTop(5);

                string[] headers =
                {
                    "Exam Date", "Student Name", "Topic Name", "Start Time", "End Time",
                    "Total Questions", "Correct Questions", "Wrong Questions", "Percentage", "Grade"
                };

                foreach (var h in headers)
                {
                    scoreTable.AddHeaderCell(new Cell()
                        .Add(new Paragraph(h).SetFont(boldFont).SetFontSize(8))
                        .SetBackgroundColor(new DeviceRgb(230, 230, 230))
                        .SetTextAlignment(TextAlignment.CENTER));
                }

                foreach (var e in examData)
                {
                    var total = e.TotalQuestions ?? e.AttemptedQuestions;
                    var percentage = total == 0 ? 0m : Math.Round((decimal)e.CorrectQuestions * 100m / total, 2);

                    scoreTable.AddCell(new Paragraph(e.ExamDate?.ToString("dd/MM/yyyy") ?? "-").SetFontSize(8));
                    scoreTable.AddCell(new Paragraph(fullName).SetFontSize(8));
                    scoreTable.AddCell(new Paragraph(e.TopicName ?? "-").SetFontSize(8));
                    scoreTable.AddCell(new Paragraph(e.StartTime?.ToString("h:mm tt") ?? "-").SetFontSize(8));
                    scoreTable.AddCell(new Paragraph(e.EndTime?.ToString("h:mm tt") ?? "-").SetFontSize(8));
                    scoreTable.AddCell(new Paragraph(total.ToString()).SetFontSize(8));
                    scoreTable.AddCell(new Paragraph(e.CorrectQuestions.ToString()).SetFontSize(8));
                    scoreTable.AddCell(new Paragraph((total - e.CorrectQuestions).ToString()).SetFontSize(8));
                    scoreTable.AddCell(new Paragraph(percentage.ToString("N2")).SetFontSize(8));
                    scoreTable.AddCell(new Paragraph(GetGrade(percentage)).SetFontSize(8));
                }

                document.Add(scoreTable);

                // Grading legend (bottom center)
                // ================= LEGEND =================
                var legend = new Table(2).UseAllAvailableWidth().SetMarginTop(10);

                legend.AddCell(new Cell().Add(new Paragraph("% Marks").SetFont(boldFont)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER));
                legend.AddCell(new Cell().Add(new Paragraph("Interpretation").SetFont(boldFont)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER));

                legend.AddCell(new Cell().Add(new Paragraph("50-59.9\n60-69.9\n70-79.9\n80-Above"))
                    .SetBorder(Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.CENTER));

                legend.AddCell(new Cell().Add(new Paragraph("Satisfactory\nFair\nGood\nExcellent"))
                    .SetBorder(Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.CENTER));

                document.Add(legend);

                document.Close();

                var base64 = Convert.ToBase64String(stream.ToArray());
                return Ok(base64);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GenerateExamReportCertificate {RegistrationId}", dto?.RegistrationId);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

    }
}
    