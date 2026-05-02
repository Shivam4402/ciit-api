using ciit_api.DTOs.Vimeo;
using ciit_api.DTOs.Youtube;

namespace ciit_api.Services.Interfaces
{
    public interface IVimeoService
    {
        Task<List<VimeoVideoResponseDto>> GetVideosByFolderAsync(string folderId);

    }
}
