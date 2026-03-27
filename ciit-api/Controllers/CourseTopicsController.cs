using ciit_api.DTOs.CourseTopic;
using ciit_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Controllers
{
    [Route("api/course-topics")]
    [ApiController]
    public class CourseTopicsController : BaseApiController
    {
        private readonly CiitstudContext _context;
        private readonly ILogger<CourseTopicsController> _logger;

        public CourseTopicsController(CiitstudContext context, ILogger<CourseTopicsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourseTopics()
        {
            try
            {
                var data = await _context.TbltrainingCourseTopics
                    .Where(x => x.Flag == 0)
                    .OrderBy(x => x.CourseTopicId)
                    .Select(x => new CourseTopicResponseDto
                    {
                        CourseTopicId = x.CourseTopicId,
                        CourseId = x.CourseId,
                        TopicId = x.TopicId
                    })
                    .ToListAsync();

                return ApiResponse(true, "Course topics fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllCourseTopics");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseTopicById(int id)
        {
            try
            {
                var data = await _context.TbltrainingCourseTopics
                    .Where(x => x.CourseTopicId == id && x.Flag == 0)
                    .Select(x => new CourseTopicResponseDto
                    {
                        CourseTopicId = x.CourseTopicId,
                        CourseId = x.CourseId,
                        TopicId = x.TopicId
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                    return ApiResponse(false, $"Record with ID {id} not found", statusCode: 404);

                return ApiResponse(true, "Course topic fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCourseTopicById {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourseTopic([FromBody] CreateCourseTopicDto dto)
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

                var topicExists = await _context.TbltrainingTopics
                    .AnyAsync(t => t.TopicId == dto.TopicId && t.Flag == 0);

                if (!topicExists)
                    return ApiResponse(false, $"Invalid TopicId {dto.TopicId}", statusCode: 400);

                var exists = await _context.TbltrainingCourseTopics
                    .AnyAsync(x => x.CourseId == dto.CourseId &&
                                   x.TopicId == dto.TopicId &&
                                   x.Flag == 0);

                if (exists)
                    return ApiResponse(false, "This course-topic mapping already exists", statusCode: 400);

                var entity = new TbltrainingCourseTopic
                {
                    CourseId = dto.CourseId,
                    TopicId = dto.TopicId,
                    Flag = 0
                };

                _context.TbltrainingCourseTopics.Add(entity);
                await _context.SaveChangesAsync();

                return ApiResponse(true, "Course topic created successfully", new
                {
                    entity.CourseTopicId,
                    entity.CourseId,
                    entity.TopicId
                }, statusCode: 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateCourseTopic");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourseTopic(int id, [FromBody] UpdateCourseTopicDto dto)
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
                var entity = await _context.TbltrainingCourseTopics
                    .Where(x => x.CourseTopicId == id && x.Flag == 0)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return ApiResponse(false, $"Record with ID {id} not found", statusCode: 404);

                var courseExists = await _context.TbltrainingCourses
                    .AnyAsync(c => c.CourseId == dto.CourseId && c.Flag == 0);

                var topicExists = await _context.TbltrainingTopics
                    .AnyAsync(t => t.TopicId == dto.TopicId && t.Flag == 0);

                if (!courseExists || !topicExists)
                    return ApiResponse(false, "Invalid CourseId or TopicId", statusCode: 400);

                entity.CourseId = dto.CourseId;
                entity.TopicId = dto.TopicId;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Course topic updated successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateCourseTopic {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseTopic(int id)
        {
            try
            {
                var entity = await _context.TbltrainingCourseTopics
                    .Where(x => x.CourseTopicId == id && x.Flag == 0)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return ApiResponse(false, $"Record with ID {id} not found", statusCode: 404);

                entity.Flag = 1;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Course topic deleted successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteCourseTopic {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }
    }
}
