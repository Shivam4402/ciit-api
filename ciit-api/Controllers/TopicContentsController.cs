using ciit_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Controllers
{
    [Route("api/topic-contents")]
    [ApiController]
    public class TopicContentsController : BaseApiController
    {
        private readonly CiitstudContext _context;
        private readonly ILogger<TopicContentsController> _logger;

        public TopicContentsController(CiitstudContext context, ILogger<TopicContentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContents()
        {
            try
            {
                var contents = await _context.TbltopicContents
                    .Where(c => c.Flag == 0)
                    .OrderBy(c => c.ContentName)
                    .Select(c => new TopicContentResponseDto
                    {
                        ContentId = c.ContentId,
                        TopicId = c.TopicId,
                        ContentName = c.ContentName
                    })
                    .ToListAsync();

                return ApiResponse(true, "Contents fetched successfully", contents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllContents");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentById(int id)
        {
            try
            {
                var content = await _context.TbltopicContents
                    .Where(c => c.ContentId == id && c.Flag == 0)
                    .Select(c => new TopicContentResponseDto
                    {
                        ContentId = c.ContentId,
                        TopicId = c.TopicId,
                        ContentName = c.ContentName
                    })
                    .FirstOrDefaultAsync();

                if (content == null)
                    return ApiResponse(false, $"Content with ID {id} not found", statusCode: 404);

                return ApiResponse(true, "Content fetched successfully", content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetContentById {ContentId}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateContent([FromBody] CreateTopicContentDto dto)
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
                var topicExists = await _context.TbltrainingTopics
                    .AnyAsync(t => t.TopicId == dto.TopicId && t.Flag == 0);

                if (!topicExists)
                    return ApiResponse(false, $"Invalid TopicId {dto.TopicId}", statusCode: 400);

                var content = new TbltopicContent
                {
                    TopicId = dto.TopicId,
                    ContentName = dto.ContentName,
                    Flag = 0
                };

                _context.TbltopicContents.Add(content);
                await _context.SaveChangesAsync();

                return ApiResponse(true, "Content created successfully", new
                {
                    content.ContentId,
                    content.ContentName,
                    content.TopicId
                }, statusCode: 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateContent");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContent(int id, [FromBody] UpdateTopicContentDto dto)
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
                var content = await _context.TbltopicContents
                    .Where(c => c.ContentId == id && c.Flag == 0)
                    .FirstOrDefaultAsync();

                if (content == null)
                    return ApiResponse(false, $"Content with ID {id} not found", statusCode: 404);

                content.ContentName = dto.ContentName;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Content updated successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateContent {ContentId}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContent(int id)
        {
            try
            {
                var content = await _context.TbltopicContents
                    .Where(c => c.ContentId == id && c.Flag == 0)
                    .FirstOrDefaultAsync();

                if (content == null)
                    return ApiResponse(false, $"Content with ID {id} not found", statusCode: 404);

                content.Flag = 1;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Content deleted successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteContent {ContentId}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }
    }

}
