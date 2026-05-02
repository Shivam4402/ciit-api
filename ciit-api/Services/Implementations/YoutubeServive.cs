using ciit_api.DTOs.Youtube;
using ciit_api.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace ciit_api.Services.Implementations
{
    public class YoutubeService : IYoutubeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IMemoryCache _cache;

        public YoutubeService(HttpClient httpClient, IOptions<YouTubeSettings> settings, IMemoryCache cache)
        {
            _cache = cache;
            _httpClient = httpClient;
            _apiKey = settings.Value.ApiKey;
        }
        public async Task<YouTubePagedResult> GetPlaylistVideosAsync(string playlistId, string pageToken = "")
        {
            var cacheKey = $"youtube:{playlistId}:{pageToken}";

            // ✅ CACHE CHECK
            if (_cache.TryGetValue(cacheKey, out YouTubePagedResult cachedData))
            {
                return cachedData;
            }

            // -------------------------
            // 1️⃣ GET VIDEOS
            // -------------------------
            var url = $"https://www.googleapis.com/youtube/v3/playlistItems" +
                            $"?part=snippet&maxResults=25&playlistId={playlistId}&key={_apiKey}";

            if (!string.IsNullOrEmpty(pageToken))
            {
                url += $"&pageToken={pageToken}";
            }

            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(json);

            var videos = new List<YouTubeVideoDto>();

            var itemsArray = result["items"];

            if (itemsArray != null && itemsArray.HasValues)
            {
                foreach (var item in itemsArray)
                {
                    videos.Add(new YouTubeVideoDto
                    {
                        Title = item["snippet"]?["title"]?.ToString(),
                        VideoId = item["snippet"]?["resourceId"]?["videoId"]?.ToString(),
                        Thumbnail = item["snippet"]?["thumbnails"]?["medium"]?["url"]?.ToString()
                    });
                }
            }

            var nextPageToken = result["nextPageToken"]?.ToString();

            // -------------------------
            // 2️⃣ GET TOTAL COUNT (NEW 🔥)
            // -------------------------
            int totalCount = 0;

            var countCacheKey = $"youtube:count:{playlistId}";

            if (_cache.TryGetValue(countCacheKey, out int cachedCount))
            {
                totalCount = cachedCount;
            }
            else
            {
                var countUrl = $"https://www.googleapis.com/youtube/v3/playlists" +
                                     $"?part=contentDetails&id={playlistId}&key={_apiKey}";

                var countResponse = await _httpClient.GetAsync(countUrl);
                var countJson = await countResponse.Content.ReadAsStringAsync();
                var countResult = JObject.Parse(countJson);

                var items = countResult["items"];

                if (items != null && items.HasValues)
                {
                    totalCount = items[0]?["contentDetails"]?["itemCount"]?.ToObject<int>() ?? 0;
                }
                else
                {
                    totalCount = 0; // invalid playlist
                }

                // cache count longer (rarely changes)
                _cache.Set(countCacheKey, totalCount, TimeSpan.FromHours(1));
            }

            // -------------------------
            // FINAL RESPONSE
            // -------------------------
            var data = new YouTubePagedResult
            {
                Videos = videos,
                NextPageToken = nextPageToken,
                TotalCount = totalCount
            };

            // cache page data
            _cache.Set(cacheKey, data, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));

            return data;
        }

    }
}
