using Microsoft.EntityFrameworkCore;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;
using WebMusic.Infrastructure.Data;

namespace WebMusic.Infrastructure.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly WebMusicDbContext _context;

        public PlaylistRepository(WebMusicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Playlist>> GetAllPlaylistsAsync()
        {
            return await _context.Playlists
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<Playlist?> GetPlaylistByIdAsync(int id)
        {
            return await _context.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                .FirstOrDefaultAsync(p => p.PlaylistId == id);
        }

        public async Task<IEnumerable<Playlist>> GetPlaylistsByUserIdAsync(int userId)
        {
            return await _context.Playlists
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Playlist>> GetPublicPlaylistsAsync()
        {
            return await _context.Playlists
                .Include(p => p.User)
                .Where(p => p.IsPublic)
                .ToListAsync();
        }

        public async Task<IEnumerable<Playlist>> SearchPlaylistsAsync(string searchTerm)
        {
            return await _context.Playlists
                .Include(p => p.User)
                .Where(p => p.PlaylistName.Contains(searchTerm) ||
                           p.Description!.Contains(searchTerm) ||
                           p.User!.UserName.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<Playlist> AddPlaylistAsync(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
            return playlist;
        }

        public async Task<Playlist> UpdatePlaylistAsync(Playlist playlist)
        {
            _context.Playlists.Update(playlist);
            await _context.SaveChangesAsync();
            return playlist;
        }

        public async Task DeletePlaylistAsync(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Playlists.AnyAsync(p => p.PlaylistId == id);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Playlists.CountAsync();
        }

        public async Task AddSongToPlaylistAsync(int playlistId, int songId)
        {
            var playlistSong = new PlaylistSong
            {
                PlaylistId = playlistId,
                SongId = songId,
                AddedAt = DateTime.UtcNow
            };

            _context.PlaylistSongs.Add(playlistSong);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveSongFromPlaylistAsync(int playlistId, int songId)
        {
            var playlistSong = await _context.PlaylistSongs
                .FirstOrDefaultAsync(ps => ps.PlaylistId == playlistId && ps.SongId == songId);

            if (playlistSong != null)
            {
                _context.PlaylistSongs.Remove(playlistSong);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Song>> GetSongsInPlaylistAsync(int playlistId)
        {
            return await _context.PlaylistSongs
                .Where(ps => ps.PlaylistId == playlistId)
                .Include(ps => ps.Song)
                    .ThenInclude(s => s.User)
                .Include(ps => ps.Song)
                    .ThenInclude(s => s.Genre)
                .Include(ps => ps.Song)
                    .ThenInclude(s => s.Album)
                .Include(ps => ps.Song)
                    .ThenInclude(s => s.Artist)
                .Select(ps => ps.Song!)
                .ToListAsync();
        }
    }
}
