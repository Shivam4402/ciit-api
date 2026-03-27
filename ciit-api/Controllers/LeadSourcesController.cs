using ciit_api.DTOs.LeadSource;
using ciit_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Controllers
{
    [Route("api/lead-sources")]
    [ApiController]
    public class LeadSourcesController : BaseApiController
    {
        private readonly CiitstudContext _context;
        private readonly ILogger<LeadSourcesController> _logger;

        public LeadSourcesController(CiitstudContext context, ILogger<LeadSourcesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLeadSources()
        {
            try
            {
                var data = await _context.TblleadSources
                    .Where(x => x.Flag == 0)
                    .OrderBy(x => x.SourceName)
                    .Select(x => new LeadSourceResponseDto
                    {
                        SourceId = x.SourceId,
                        SourceName = x.SourceName
                    })
                    .ToListAsync();

                return ApiResponse(true, "Lead sources fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllLeadSources");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeadSourceById(int id)
        {
            try
            {
                var data = await _context.TblleadSources
                    .Where(x => x.SourceId == id && x.Flag == 0)
                    .Select(x => new LeadSourceResponseDto
                    {
                        SourceId = x.SourceId,
                        SourceName = x.SourceName
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                    return ApiResponse(false, $"Lead source with ID {id} not found", statusCode: 404);

                return ApiResponse(true, "Lead source fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLeadSourceById {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeadSource([FromBody] CreateLeadSourceDto dto)
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
                var exists = await _context.TblleadSources
                    .AnyAsync(x => x.SourceName == dto.SourceName && x.Flag == 0);

                if (exists)
                    return ApiResponse(false, "Lead source already exists", statusCode: 400);

                var entity = new TblleadSource
                {
                    SourceName = dto.SourceName,
                    Flag = 0
                };

                _context.TblleadSources.Add(entity);
                await _context.SaveChangesAsync();

                return ApiResponse(true, "Lead source created successfully", new
                {
                    entity.SourceId,
                    entity.SourceName
                }, statusCode: 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateLeadSource");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeadSource(int id, [FromBody] UpdateLeadSourceDto dto)
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
                var entity = await _context.TblleadSources
                    .Where(x => x.SourceId == id && x.Flag == 0)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return ApiResponse(false, $"Lead source with ID {id} not found", statusCode: 404);

                entity.SourceName = dto.SourceName;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Lead source updated successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateLeadSource {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeadSource(int id)
        {
            try
            {
                var entity = await _context.TblleadSources
                    .Where(x => x.SourceId == id && x.Flag == 0)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return ApiResponse(false, $"Lead source with ID {id} not found", statusCode: 404);

                entity.Flag = 1;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Lead source deleted successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteLeadSource {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }
    }

}
