using ciit_api.DTOs.Youtube;

namespace ciit_api.Services.Interfaces
{
    public interface IYoutubeService
    {
        Task<YouTubePagedResult> GetPlaylistVideosAsync(string playlistId, string pageToken = "");
    }
}
