using WebMusic.Domain.Entities;

namespace WebMusic.Domain.Interfaces
{
    public interface IAlbumRepository
    {
        Task<IEnumerable<Album>> GetAllAlbumsAsync();
        Task<Album?> GetAlbumByIdAsync(int id);
        Task<IEnumerable<Album>> GetAlbumsByUserIdAsync(int userId);
        Task<IEnumerable<Album>> GetAlbumsByArtistIdAsync(int artistId);
        Task<IEnumerable<Album>> SearchAlbumsAsync(string searchTerm);
        Task<Album> AddAlbumAsync(Album album);
        Task<Album> UpdateAlbumAsync(Album album);
        Task DeleteAlbumAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetTotalCountAsync();
    }
}
