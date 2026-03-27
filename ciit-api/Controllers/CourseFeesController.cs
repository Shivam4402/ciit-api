using ciit_api.DTOs.CourseFee;
using ciit_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Controllers
{
    [Route("api/course-fees")]
    [ApiController]
    public class CourseFeesController : BaseApiController
    {
        private readonly CiitstudContext _context;
        private readonly ILogger<CourseFeesController> _logger;

        public CourseFeesController(CiitstudContext context, ILogger<CourseFeesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFees()
        {
            try
            {
                var data = await _context.TbltrainingCourseFees
                    .Where(x => x.Flag == 0)
                    .OrderBy(x => x.FeeId)
                    .Select(x => new CourseFeeResponseDto
                    {
                        FeeId = x.FeeId,
                        CourseId = x.CourseId,
                        FeesAmount = x.FeesAmount,
                        Gst = x.Gst,
                        FeeMode = x.FeeMode,
                        FeesChangeDate = x.FeesChangeDate
                    })
                    .ToListAsync();

                return ApiResponse(true, "Course fees fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllFees");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeeById(int id)
        {
            try
            {
                var data = await _context.TbltrainingCourseFees
                    .Where(x => x.FeeId == id && x.Flag == 0)
                    .Select(x => new CourseFeeResponseDto
                    {
                        FeeId = x.FeeId,
                        CourseId = x.CourseId,
                        FeesAmount = x.FeesAmount,
                        Gst = x.Gst,
                        FeeMode = x.FeeMode,
                        FeesChangeDate = x.FeesChangeDate
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                    return ApiResponse(false, $"Fee with ID {id} not found", statusCode: 404);

                return ApiResponse(true, "Course fee fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetFeeById {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFee([FromBody] CreateCourseFeeDto dto)
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
                var courseExists = await _context.TbltrainingCourses
                    .AnyAsync(c => c.CourseId == dto.CourseId && c.Flag == 0);

                if (!courseExists)
                    return ApiResponse(false, $"Invalid CourseId {dto.CourseId}", statusCode: 400);

                var entity = new TbltrainingCourseFee
                {
                    CourseId = dto.CourseId,
                    FeesAmount = dto.FeesAmount,
                    Gst = dto.Gst,
                    FeeMode = dto.FeeMode,
                    FeesChangeDate=DateOnly.FromDateTime(DateTime.UtcNow),
                    Flag = 0
                };

                _context.TbltrainingCourseFees.Add(entity);
                await _context.SaveChangesAsync();

                return ApiResponse(true, "Course fee created successfully", new
                {
                    entity.FeeId,
                    entity.CourseId,
                    entity.FeesAmount
                }, statusCode: 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateFee");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFee(int id, [FromBody] UpdateCourseFeeDto dto)
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
                var entity = await _context.TbltrainingCourseFees
                    .Where(x => x.FeeId == id && x.Flag == 0)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return ApiResponse(false, $"Fee with ID {id} not found", statusCode: 404);

                entity.FeesAmount = dto.FeesAmount;
                entity.Gst = dto.Gst;
                entity.FeeMode = dto.FeeMode;
                entity.FeesChangeDate = DateOnly.FromDateTime(DateTime.UtcNow);


                await _context.SaveChangesAsync();

                return ApiResponse(true, "Course fee updated successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateFee {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFee(int id)
        {
            try
            {
                var entity = await _context.TbltrainingCourseFees
                    .Where(x => x.FeeId == id && x.Flag == 0)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return ApiResponse(false, $"Fee with ID {id} not found", statusCode: 404);

                entity.Flag = 1;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Course fee deleted successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteFee {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }
    }

}
