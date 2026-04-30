using ciit_api.DTOs.StudentRegistration;
using ciit_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Controllers
{
    [Route("api/student-registrations")]
    [ApiController]
    public class StudentRegistrationController : BaseApiController
    {
        private readonly CiitstudContext _context;
        private readonly ILogger<StudentRegistrationController> _logger;

        public StudentRegistrationController(CiitstudContext context, ILogger<StudentRegistrationController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudentRegistrations()
        {
            try
            {
                var data = await _context.TblstudentRegistrations
                    .Where(x => x.Flag == 0)
                    .OrderBy(x => x.RegistrationId)
                    .Select(x => new StudentRegistrationResponseDto
                    {
                        RegistrationId = x.RegistrationId,
                        StudentId = x.StudentId,
                        RegistrationDate = x.RegistrationDate,
                        Discount = x.Discount,
                        FeeId = x.FeeId,
                        CurrentStatus = x.CurrentStatus,
                        StudentName = x.Student != null ? x.Student.StudentName : null,
                        CourseId = x.Fee != null ? x.Fee.CourseId : null,
                        CourseName = x.Fee != null && x.Fee.Course != null ? x.Fee.Course.CourseName : null
                    })
                    .ToListAsync();

                return ApiResponse(true, "Student registrations fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllStudentRegistrations");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentRegistrationById(int id)
        {
            try
            {
                var data = await _context.TblstudentRegistrations
                    .Where(x => x.RegistrationId == id && x.Flag == 0)
                    .Select(x => new StudentRegistrationResponseDto
                    {
                        RegistrationId = x.RegistrationId,
                        StudentId = x.StudentId,
                        RegistrationDate = x.RegistrationDate,
                        Discount = x.Discount,
                        FeeId = x.FeeId,
                        CurrentStatus = x.CurrentStatus,
                        StudentName = x.Student != null ? x.Student.StudentName : null,
                        CourseId = x.Fee != null ? x.Fee.CourseId : null,
                        CourseName = x.Fee != null && x.Fee.Course != null ? x.Fee.Course.CourseName : null
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                    return ApiResponse(false, $"Student registration with ID {id} not found", statusCode: 404);

                return ApiResponse(true, "Student registration fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetStudentRegistrationById {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }



        [HttpGet("student-registration-details/{studentId}")]
        public async Task<IActionResult> GetStudentRegistrationByStudentId(int studentId)
        {
            try
            {
                var data = await _context.TblstudentRegistrations
                    .Where(x => x.StudentId == studentId && x.Flag == 0)
                    .Select(x => new StudentRegistrationResponseDto
                    {
                        RegistrationId = x.RegistrationId,
                        StudentId = x.StudentId,
                        RegistrationDate = x.RegistrationDate,
                        Discount = x.Discount,
                        FeeId = x.FeeId,
                        CurrentStatus = x.CurrentStatus,
                        StudentName = x.Student != null ? x.Student.StudentName : null,
                        CourseId = x.Fee != null ? x.Fee.CourseId : null,
                        CourseName = x.Fee != null && x.Fee.Course != null ? x.Fee.Course.CourseName : null
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                    return ApiResponse(false, $"Student registration with ID {studentId} not found", statusCode: 404);

                return ApiResponse(true, "Student registration fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetStudentRegistrationByStudentId {studentId}", studentId);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudentRegistration([FromBody] CreateStudentRegistrationDto dto)
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
                var entity = new TblstudentRegistration
                {
                    StudentId = dto.StudentId,
                    RegistrationDate = dto.RegistrationDate ?? DateTime.UtcNow,
                    Discount = dto.Discount,
                    FeeId = dto.FeeId,
                    CurrentStatus = dto.CurrentStatus ?? "Registered",
                    Flag = 0
                };

                _context.TblstudentRegistrations.Add(entity);
                await _context.SaveChangesAsync();

                return ApiResponse(true, "Student registration created successfully", new
                {
                    entity.RegistrationId,
                    entity.StudentId
                }, statusCode: 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateStudentRegistration");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudentRegistration(int id, [FromBody] UpdateStudentRegistrationDto dto)
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
                var entity = await _context.TblstudentRegistrations
                    .Where(x => x.RegistrationId == id && x.Flag == 0)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return ApiResponse(false, $"Student registration with ID {id} not found", statusCode: 404);

                entity.StudentId = dto.StudentId;
                entity.RegistrationDate = dto.RegistrationDate;
                entity.Discount = dto.Discount;
                entity.FeeId = dto.FeeId;
                entity.CurrentStatus = dto.CurrentStatus;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Student registration updated successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateStudentRegistration {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentRegistration(int id)
        {
            try
            {
                var entity = await _context.TblstudentRegistrations
                    .Where(x => x.RegistrationId == id && x.Flag == 0)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return ApiResponse(false, $"Student registration with ID {id} not found", statusCode: 404);

                entity.Flag = 1;
                await _context.SaveChangesAsync();

                return ApiResponse(true, "Student registration deleted successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteStudentRegistration {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }
    }
}
