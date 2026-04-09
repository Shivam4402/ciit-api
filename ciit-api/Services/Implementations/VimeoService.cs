using ciit_api.DTOs.CourseVideo;
using ciit_api.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ciit_api.Services.Implementations
{
    public class VimeoService : IVimeoService
    {
        private readonly HttpClient _httpClient;

        public VimeoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<VimeoVideoResponseDto>> GetVideosByFolderAsync(string folderId)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://api.vimeo.com/me/projects/{folderId}/videos"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", "eac378a5725d5ecc750c63e0ee36e248");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(json);

            var videos = new List<VimeoVideoResponseDto>();

            foreach (var item in result["data"])
            {
                var uri = item["uri"]?.ToString(); // "/videos/1181059365"

                var video = new VimeoVideoResponseDto
                {
                    Title = item["name"]?.ToString(),
                    VideoId = uri?.Replace("/videos/", ""),
                    EmbedUrl = item["player_embed_url"]?.ToString(),
                    Duration = item["duration"]?.ToObject<int>() ?? 0,
                    Thumbnail = item["pictures"]?["sizes"]?[3]?["link"]?.ToString()
                };

                videos.Add(video);
            }

            return videos;
        }
    }
}
