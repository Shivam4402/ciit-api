using ciit_api.DTOs.CourseVideo;

namespace ciit_api.Services.Interfaces
{
    public interface IVimeoService
    {
        Task<List<VimeoVideoResponseDto>> GetVideosByFolderAsync(string folderId);

    }
}
