using ciit_api.DTOs.Course;
using ciit_api.DTOs.CourseFee;
using ciit_api.DTOs.Topic;
using ciit_api.DTOs.TopicContents;
using ciit_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Controllers
{
    //[Authorize]
    [Route("api/training-courses")]
    [ApiController]
    public class TrainingCoursesController : BaseApiController
    {
        private readonly CiitstudContext _context;
        private readonly ILogger<TrainingCoursesController> _logger;

        public TrainingCoursesController(CiitstudContext context, ILogger<TrainingCoursesController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            try
            {

                var courses = await _context.TbltrainingCourses
                  .Include(c => c.TbltrainingCourseFees)
                  .Include(c => c.TbltrainingCourseTopics)
                  .Where(c => c.Flag == 0)
                  .OrderBy(c => c.CourseName)
                  .Select(c => new TrainingCourseResponseDto
                  {
                      CourseId = c.CourseId,
                      CourseName = c.CourseName,

                  })
                  .ToListAsync();

                return ApiResponse(true, "Courses fetched successfully", courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllCourses");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            try
            {
                var course = await _context.TbltrainingCourses
                 .Include(c => c.TbltrainingCourseFees)
                 .Include(c => c.TbltrainingCourseTopics)
                 .Where(c => c.CourseId == id && c.Flag == 0)
                 .OrderBy(c => c.CourseName)
                 .Select(c => new TrainingCourseResponseDto
                 {
                     CourseId = c.CourseId,
                     CourseName = c.CourseName,

                 })
                 .FirstOrDefaultAsync();

                if (course == null)
                {
                    return ApiResponse(false, $"Course with ID {id} not found", statusCode: 404);
                }

                return ApiResponse(true, "Course fetched successfully", course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCourseById {CourseId}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateTrainingCourseDto dto)
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
                var course = new TbltrainingCourse
                {
                    CourseName = dto.CourseName,
                    Flag = 0,

                };

                _context.TbltrainingCourses.Add(course);
                await _context.SaveChangesAsync();

                return ApiResponse(true, "Course created successfully", new
                {
                    course.CourseId,
                    course.CourseName
                }, statusCode: 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateCourse");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateTrainingCourseDto dto)
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
                var course = await _context.TbltrainingCourses
                    .Where(p => p.CourseId == id && p.Flag == 0)
                    .FirstOrDefaultAsync();

                if (course == null)
                    return ApiResponse(false, $"Course with ID {id} not found", statusCode: 404);

                course.CourseName = dto.CourseName;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Course updated successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateCourse {CourseId}", id);
                return ApiResponse(false, "Something went wrong",
                    error: ex.Message, statusCode: 500);
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {

                var course = await _context.TbltrainingCourses
                    .Where(p =>
                        p.CourseId == id &&
                        p.Flag == 0)
                    .FirstOrDefaultAsync();

                if (course == null)
                    return ApiResponse(false, $"Course with ID {id} not found", statusCode: 404);

                course.Flag = 1;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Course deleted successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteCourse {CourseId}", id);
                return ApiResponse(false, "Something went wrong",
                    error: ex.Message, statusCode: 500);
            }
        }

        //  Course wise fees
        [HttpGet("{courseId:int}/fees")]
        public async Task<IActionResult> GetCourseFees(int courseId)
        {
            try
            {
                var courseExists = await _context.TbltrainingCourses
                    .AnyAsync(c => c.CourseId == courseId && c.Flag == 0);

                if (!courseExists)
                    return ApiResponse(false, $"Course with ID {courseId} not found", statusCode: 404);

                var fees = await _context.TbltrainingCourseFees
                    .Where(f => f.CourseId == courseId && f.Flag == 0)
                    .OrderByDescending(f => f.FeesChangeDate)
                    .ThenByDescending(f => f.FeeId)
                    .Select(f => new CourseFeeResponseDto
                    {
                        FeeId = f.FeeId,
                        CourseId = f.CourseId,
                        FeesAmount = f.FeesAmount,
                        Gst = f.Gst,
                        FeeMode = f.FeeMode,
                        FeesChangeDate = f.FeesChangeDate
                    })
                    .ToListAsync();

                return ApiResponse(true, "Course fees fetched successfully", fees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCourseFees {CourseId}", courseId);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        //  Course wise topics
        [HttpGet("{courseId:int}/topics")]
        public async Task<IActionResult> GetCourseTopics(int courseId)
        {
            try
            {
                var courseExists = await _context.TbltrainingCourses
                    .AnyAsync(c => c.CourseId == courseId && c.Flag == 0);

                if (!courseExists)
                    return ApiResponse(false, $"Course with ID {courseId} not found", statusCode: 404);

                var topics = await _context.TbltrainingCourseTopics
                    .Where(ct => ct.CourseId == courseId && ct.Flag == 0 && ct.Topic != null && ct.Topic.Flag == 0)
                    .OrderBy(ct => ct.Topic!.TopicName)
                    .Select(ct => new TrainingTopicResponseDto
                    {
                        TopicId = ct.Topic!.TopicId,
                        TopicName = ct.Topic.TopicName,
                        Publicfolderid = ct.Topic.Publicfolderid
                    })
                    .ToListAsync();

                return ApiResponse(true, "Course topics fetched successfully", topics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCourseTopics {CourseId}", courseId);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        // Course wise topics & topic wise contents
        [HttpGet("{courseId:int}/topics-with-contents")]
        public async Task<IActionResult> GetCourseTopicsWithContents(int courseId)
        {
            try
            {
                var courseExists = await _context.TbltrainingCourses
                    .AnyAsync(c => c.CourseId == courseId && c.Flag == 0);

                if (!courseExists)
                    return ApiResponse(false, $"Course with ID {courseId} not found", statusCode: 404);

                var data = await _context.TbltrainingCourseTopics
                    .Where(ct => ct.CourseId == courseId && ct.Flag == 0 && ct.Topic != null && ct.Topic.Flag == 0)
                    .Select(ct => new TopicWithContentsResponseDto
                    {
                        TopicId = ct.Topic!.TopicId,
                        TopicName = ct.Topic.TopicName,
                        Contents = ct.Topic.TbltopicContents
                            .Where(c => c.Flag == 0)
                            .OrderBy(c => c.ContentName)
                            .Select(c => new TopicContentResponseDto
                            {
                                ContentId = c.ContentId,
                                TopicId = c.TopicId,
                                ContentName = c.ContentName
                            })
                            .ToList()
                    })
                    .OrderBy(x => x.TopicName)
                    .ToListAsync();

                return ApiResponse(true, "Course topics with contents fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCourseTopicsWithContents {CourseId}", courseId);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }


    }
}
