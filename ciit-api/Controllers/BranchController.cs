using ciit_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Controllers
{
    [Route("api/branches")]
    [ApiController]
    public class BranchController : BaseApiController
    {
        private readonly CiitstudContext _context;
        private readonly ILogger<BranchController> _logger;

        public BranchController(CiitstudContext context, ILogger<BranchController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBranches()
        {
            try
            {
                var data = await _context.Tblbranches
                    .OrderBy(x => x.BranchId)
                    .Select(x => new BranchResponseDto
                    {
                        BranchId = x.BranchId,
                        BranchName = x.BranchName
                    })
                    .ToListAsync();

                return ApiResponse(true, "Branches fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllBranches");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }


    }
}
