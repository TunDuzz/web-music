using WebMusic.Web.ViewModels.Songs;
using WebMusic.Application.DTOs;

namespace WebMusic.Web.Services
{
    /// <summary>
    /// Service interface cho xử lý business logic của Songs trong Web layer
    /// </summary>
    public interface ISongWebService
    {
        Task<SongListViewModel> GetSongsAsync(SongListViewModel model);
        Task<SongCreateViewModel> GetCreateViewModelAsync();
        Task<SongEditViewModel> GetEditViewModelAsync(int id);
        Task<bool> CreateSongAsync(SongCreateViewModel model);
        Task<bool> UpdateSongAsync(SongEditViewModel model);
        Task<bool> DeleteSongAsync(int id);
        Task<SongDto?> GetSongByIdAsync(int id);
    }
}
