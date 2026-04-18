using ciit_api.DTOs.CourseVideo;
using ciit_api.DTOs.Vimeo;
using ciit_api.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
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
        private readonly IMemoryCache _cache;
        private readonly string _accessToken;



        public VimeoService(HttpClient httpClient, IMemoryCache cache, IOptions<VimeoSettings> vimeoOptions)
        {
            _httpClient = httpClient;
            _cache = cache;
            _accessToken = vimeoOptions.Value.AccessToken;

        }


        public async Task<List<VimeoVideoResponseDto>> GetVideosByFolderAsync(string folderId)
        {
            string cacheKey = $"vimeo_folder_{folderId}";

            // ✅ 1. Try to get data from cache
            if (_cache.TryGetValue(cacheKey, out List<VimeoVideoResponseDto> cachedVideos))
            {
                Console.WriteLine("CACHE HIT ✅");
                return cachedVideos; // ⚡ FAST RESPONSE (no API call)
            }


            Console.WriteLine("CACHE MISS ❌");
            // ❌ 2. Cache MISS → Call Vimeo API
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://api.vimeo.com/me/projects/{folderId}/videos"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(json);

            var videos = new List<VimeoVideoResponseDto>();

            foreach (var item in result["data"])
            {
                var uri = item["uri"]?.ToString();

                videos.Add(new VimeoVideoResponseDto
                {
                    Title = item["name"]?.ToString(),
                    VideoId = uri?.Replace("/videos/", ""),
                    EmbedUrl = item["player_embed_url"]?.ToString(),
                    Duration = item["duration"]?.ToObject<int>() ?? 0,
                    Thumbnail = item["pictures"]?["sizes"]?[3]?["link"]?.ToString()
                });
            }

            // ✅ 3. Store data in cache
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); // ⏱️ Cache for 10 mins

            _cache.Set(cacheKey, videos, cacheOptions);

            return videos;
        }



        //public async Task<List<VimeoVideoResponseDto>> GetVideosByFolderAsync(string folderId)
        //{
        //    var request = new HttpRequestMessage(
        //        HttpMethod.Get,
        //        $"https://api.vimeo.com/me/projects/{folderId}/videos"
        //    );

        //    request.Headers.Authorization =
        //        new AuthenticationHeaderValue("Bearer", "eac378a5725d5ecc750c63e0ee36e248");

        //    var response = await _httpClient.SendAsync(request);
        //    response.EnsureSuccessStatusCode();

        //    var json = await response.Content.ReadAsStringAsync();
        //    var result = JObject.Parse(json);

        //    var videos = new List<VimeoVideoResponseDto>();

        //    foreach (var item in result["data"])
        //    {
        //        var uri = item["uri"]?.ToString(); // "/videos/1181059365"

        //        var video = new VimeoVideoResponseDto
        //        {
        //            Title = item["name"]?.ToString(),
        //            VideoId = uri?.Replace("/videos/", ""),
        //            EmbedUrl = item["player_embed_url"]?.ToString(),
        //            Duration = item["duration"]?.ToObject<int>() ?? 0,
        //            Thumbnail = item["pictures"]?["sizes"]?[3]?["link"]?.ToString()
        //        };

        //        videos.Add(video);
        //    }

        //    return videos;
        //}
    }
}
