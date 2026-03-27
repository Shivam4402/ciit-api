using ciit_api.DTOs.Qualification;
using ciit_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Controllers
{
    [Route("api/qualifications")]
    [ApiController]
    public class QualificationController : BaseApiController
    {
        private readonly CiitstudContext _context;
        private readonly ILogger<QualificationController> _logger;

        public QualificationController(CiitstudContext context, ILogger<QualificationController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllQualifications()
        {
            try
            {
                var data = await _context.Tblqualifications
                    .Where(x => x.Flag == 0)
                    .OrderBy(x => x.QualificationId)
                    .Select(x => new QualificationResponseDto
                    {
                        QualificationId = x.QualificationId,
                        Qualification = x.Qualification
                    })
                    .ToListAsync();

                return ApiResponse(true, "Qualifications fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllQualifications");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }
    }
}
