using WebMusic.Web.ViewModels.Playlists;
using WebMusic.Application.DTOs;

namespace WebMusic.Web.Services
{
    /// <summary>
    /// Service interface cho xử lý business logic của Playlists trong Web layer
    /// </summary>
    public interface IPlaylistWebService
    {
        Task<PlaylistListViewModel> GetPlaylistsAsync(PlaylistListViewModel model);
        Task<PlaylistCreateViewModel> GetCreateViewModelAsync();
        Task<PlaylistEditViewModel> GetEditViewModelAsync(int id);
        Task<bool> CreatePlaylistAsync(PlaylistCreateViewModel model);
        Task<bool> UpdatePlaylistAsync(PlaylistEditViewModel model);
        Task<bool> DeletePlaylistAsync(int id);
        Task<PlaylistDto?> GetPlaylistByIdAsync(int id);
    }
}
