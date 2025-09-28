using WebMusic.Domain.Entities;

namespace WebMusic.Domain.Interfaces
{
    public interface ISongRepository
    {
        Task<IEnumerable<Song>> GetAllSongsAsync();
        Task<Song?> GetSongByIdAsync(int id);
        Task<IEnumerable<Song>> GetSongsByUserIdAsync(int userId);
        Task<IEnumerable<Song>> GetSongsByGenreIdAsync(int genreId);
        Task<IEnumerable<Song>> GetSongsByAlbumIdAsync(int albumId);
        Task<IEnumerable<Song>> GetSongsByArtistIdAsync(int artistId);
        Task<IEnumerable<Song>> SearchSongsAsync(string searchTerm);
        Task<IEnumerable<Song>> GetPopularSongsAsync(int count);
        Task<IEnumerable<Song>> GetRecentSongsAsync(int count);
        Task<Song> AddSongAsync(Song song);
        Task<Song> UpdateSongAsync(Song song);
        Task DeleteSongAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetTotalCountAsync();
        Task ApproveSongAsync(int id);
        Task RejectSongAsync(int id);
    }
}
