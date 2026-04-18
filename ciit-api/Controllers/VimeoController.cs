using ciit_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ciit_api.Controllers
{

    [Authorize]
    [Route("api/vimeo")]
    [ApiController]
    public class VimeoController : BaseApiController
    {
        private readonly IVimeoService _service;
        private readonly ILogger<VimeoController> _logger;

        public VimeoController(IVimeoService service, ILogger<VimeoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("folder/{folderId}")]
        public async Task<IActionResult> GetVideosByFolder(string folderId)
        {
            try
            {
                var data = await _service.GetVideosByFolderAsync(folderId);

                if (data == null || data.Count == 0)
                    return ApiResponse(false, "No videos found", statusCode: 404);

                return ApiResponse(true, "Videos fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetVideosByFolder {FolderId}", folderId);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }
    }
}

