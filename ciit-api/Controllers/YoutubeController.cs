using ciit_api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ciit_api.Controllers
{
    [Route("api/youtube")]
    [ApiController]
    public class YoutubeController : BaseApiController
    {
        private readonly IYoutubeService _service;
        private readonly ILogger<YoutubeController> _logger;

        public YoutubeController(IYoutubeService service, ILogger<YoutubeController> logger)
        {
            _service = service;
            _logger = logger;
        }


        [HttpGet("video-playlist/{playlistId}")]
        public async Task<IActionResult> GetVideos(string playlistId, string pageToken = "")
        {
            try
            {
                var result = await _service.GetPlaylistVideosAsync(playlistId, pageToken);

                if (result.Videos == null || result.Videos.Count == 0)
                    return ApiResponse(false, "No videos found", statusCode: 404);

                return ApiResponse(true, "Videos fetched successfully", new
                {
                    videos = result.Videos,
                    nextPageToken = result.NextPageToken,
                    totalCount = result.TotalCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPlaylistVideos {PlaylistId}", playlistId);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }
    }
}
