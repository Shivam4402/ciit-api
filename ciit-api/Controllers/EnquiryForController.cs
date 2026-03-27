using ciit_api.DTOs.EnquiryFor;
using ciit_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Controllers
{
    [Route("api/enquiry-for")]
    [ApiController]
    public class EnquiryForController : BaseApiController
    {
        private readonly CiitstudContext _context;
        private readonly ILogger<EnquiryForController> _logger;

        public EnquiryForController(CiitstudContext context, ILogger<EnquiryForController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEnquiryFor()
        {
            try
            {
                var data = await _context.TblenquiryFors
                    .Where(x => x.Flag == 0)
                    .OrderBy(x => x.EnquiryFor)
                    .Select(x => new EnquiryForResponseDto
                    {
                        EnquiryForId = x.EnquiryForId,
                        EnquiryFor = x.EnquiryFor
                    })
                    .ToListAsync();

                return ApiResponse(true, "Enquiry types fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllEnquiryFor");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnquiryForById(int id)
        {
            try
            {
                var data = await _context.TblenquiryFors
                    .Where(x => x.EnquiryForId == id && x.Flag == 0)
                    .Select(x => new EnquiryForResponseDto
                    {
                        EnquiryForId = x.EnquiryForId,
                        EnquiryFor = x.EnquiryFor
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                    return ApiResponse(false, $"Record with ID {id} not found", statusCode: 404);

                return ApiResponse(true, "Enquiry type fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetEnquiryForById {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnquiryFor([FromBody] CreateEnquiryForDto dto)
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
                var exists = await _context.TblenquiryFors
                    .AnyAsync(x => x.EnquiryFor == dto.EnquiryFor && x.Flag == 0);

                if (exists)
                    return ApiResponse(false, "Enquiry type already exists", statusCode: 400);

                var entity = new TblenquiryFor
                {
                    EnquiryFor = dto.EnquiryFor,
                    Flag = 0
                };

                _context.TblenquiryFors.Add(entity);
                await _context.SaveChangesAsync();

                return ApiResponse(true, "Enquiry type created successfully", new
                {
                    entity.EnquiryForId,
                    entity.EnquiryFor
                }, statusCode: 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateEnquiryFor");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnquiryFor(int id, [FromBody] UpdateEnquiryForDto dto)
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
                var entity = await _context.TblenquiryFors
                    .Where(x => x.EnquiryForId == id && x.Flag == 0)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return ApiResponse(false, $"Record with ID {id} not found", statusCode: 404);

                entity.EnquiryFor = dto.EnquiryFor;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Enquiry type updated successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateEnquiryFor {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnquiryFor(int id)
        {
            try
            {
                var entity = await _context.TblenquiryFors
                    .Where(x => x.EnquiryForId == id && x.Flag == 0)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return ApiResponse(false, $"Record with ID {id} not found", statusCode: 404);

                entity.Flag = 1;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Enquiry type deleted successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteEnquiryFor {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }
    }
}
