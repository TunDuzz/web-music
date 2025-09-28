using WebMusic.Domain.Entities;

namespace WebMusic.Domain.Interfaces
{
    public interface IPlaylistRepository
    {
        Task<IEnumerable<Playlist>> GetAllPlaylistsAsync();
        Task<Playlist?> GetPlaylistByIdAsync(int id);
        Task<IEnumerable<Playlist>> GetPlaylistsByUserIdAsync(int userId);
        Task<IEnumerable<Playlist>> GetPublicPlaylistsAsync();
        Task<IEnumerable<Playlist>> SearchPlaylistsAsync(string searchTerm);
        Task<Playlist> AddPlaylistAsync(Playlist playlist);
        Task<Playlist> UpdatePlaylistAsync(Playlist playlist);
        Task DeletePlaylistAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetTotalCountAsync();
        Task AddSongToPlaylistAsync(int playlistId, int songId);
        Task RemoveSongFromPlaylistAsync(int playlistId, int songId);
        Task<IEnumerable<Song>> GetSongsInPlaylistAsync(int playlistId);
    }
}
