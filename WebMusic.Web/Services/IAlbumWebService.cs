using WebMusic.Web.ViewModels.Albums;
using WebMusic.Application.DTOs;

namespace WebMusic.Web.Services
{
    /// <summary>
    /// Service interface cho xử lý business logic của Albums trong Web layer
    /// </summary>
    public interface IAlbumWebService
    {
        Task<AlbumListViewModel> GetAlbumsAsync(AlbumListViewModel model);
        Task<AlbumCreateViewModel> GetCreateViewModelAsync();
        Task<AlbumEditViewModel> GetEditViewModelAsync(int id);
        Task<bool> CreateAlbumAsync(AlbumCreateViewModel model);
        Task<bool> UpdateAlbumAsync(AlbumEditViewModel model);
        Task<bool> DeleteAlbumAsync(int id);
        Task<AlbumDto?> GetAlbumByIdAsync(int id);
    }
}
