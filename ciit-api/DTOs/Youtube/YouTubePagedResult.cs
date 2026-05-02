namespace ciit_api.DTOs.Youtube
{
    public class YouTubePagedResult
    {
        public List<YouTubeVideoDto> Videos { get; set; }
        public string NextPageToken { get; set; }
        public int TotalCount { get; set; }
    }

}
