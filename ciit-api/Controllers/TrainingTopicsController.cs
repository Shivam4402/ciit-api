using ciit_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Controllers
{
    [Route("api/training-topics")]
    [ApiController]
    public class TrainingTopicsController : BaseApiController
    {
        private readonly CiitstudContext _context;
        private readonly ILogger<TrainingTopicsController> _logger;

        public TrainingTopicsController(CiitstudContext context, ILogger<TrainingTopicsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTopics()
        {
            try
            {
                var topics = await _context.TbltrainingTopics
                    .Where(t => t.Flag == 0)
                    .OrderBy(t => t.TopicName)
                    .Select(t => new TrainingTopicResponseDto
                    {
                        TopicId = t.TopicId,
                        TopicName = t.TopicName,
                        Publicfolderid = t.Publicfolderid
                    })
                    .ToListAsync();

                return ApiResponse(true, "Topics fetched successfully", topics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllTopics");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopicById(int id)
        {
            try
            {
                var topic = await _context.TbltrainingTopics
                    .Where(t => t.TopicId == id && t.Flag == 0)
                    .Select(t => new TrainingTopicResponseDto
                    {
                        TopicId = t.TopicId,
                        TopicName = t.TopicName,
                        Publicfolderid = t.Publicfolderid
                    })
                    .FirstOrDefaultAsync();

                if (topic == null)
                    return ApiResponse(false, $"Topic with ID {id} not found", statusCode: 404);

                return ApiResponse(true, "Topic fetched successfully", topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTopicById {TopicId}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic([FromBody] CreateTrainingTopicDto dto)
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
                var topic = new TbltrainingTopic
                {
                    TopicName = dto.TopicName,
                    Flag = 0,
                    Publicfolderid = null
                };

                _context.TbltrainingTopics.Add(topic);
                await _context.SaveChangesAsync();

                return ApiResponse(true, "Topic created successfully", new
                {
                    topic.TopicId,
                    topic.TopicName
                }, statusCode: 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateTopic");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopic(int id, [FromBody] UpdateTrainingTopicDto dto)
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
                var topic = await _context.TbltrainingTopics
                    .Where(t => t.TopicId == id && t.Flag == 0)
                    .FirstOrDefaultAsync();

                if (topic == null)
                    return ApiResponse(false, $"Topic with ID {id} not found", statusCode: 404);

                topic.TopicName = dto.TopicName;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Topic updated successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateTopic {TopicId}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            try
            {
                var topic = await _context.TbltrainingTopics
                    .Where(t => t.TopicId == id && t.Flag == 0)
                    .FirstOrDefaultAsync();

                if (topic == null)
                    return ApiResponse(false, $"Topic with ID {id} not found", statusCode: 404);

                topic.Flag = 1;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Topic deleted successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteTopic {TopicId}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }


        // Topic wise contents
        [HttpGet("{topicId:int}/contents")]
        public async Task<IActionResult> GetTopicContents(int topicId)
        {
            try
            {
                var topicExists = await _context.TbltrainingTopics
                    .AnyAsync(t => t.TopicId == topicId && t.Flag == 0);

                if (!topicExists)
                    return ApiResponse(false, $"Topic with ID {topicId} not found", statusCode: 404);

                var contents = await _context.TbltopicContents
                    .Where(c => c.TopicId == topicId && c.Flag == 0)
                    .OrderBy(c => c.ContentName)
                    .Select(c => new TopicContentResponseDto
                    {
                        ContentId = c.ContentId,
                        TopicId = c.TopicId,
                        ContentName = c.ContentName
                    })
                    .ToListAsync();

                return ApiResponse(true, "Topic contents fetched successfully", contents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTopicContents {TopicId}", topicId);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }
    }
}
