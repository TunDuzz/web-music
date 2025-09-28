using Microsoft.EntityFrameworkCore;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;
using WebMusic.Infrastructure.Data;

namespace WebMusic.Infrastructure.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly WebMusicDbContext _context;

        public SongRepository(WebMusicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Song>> GetAllSongsAsync()
        {
            return await _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .ToListAsync();
        }

        public async Task<Song?> GetSongByIdAsync(int id)
        {
            return await _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .FirstOrDefaultAsync(s => s.SongId == id);
        }

        public async Task<IEnumerable<Song>> GetSongsByUserIdAsync(int userId)
        {
            return await _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetSongsByGenreIdAsync(int genreId)
        {
            return await _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .Where(s => s.GenreId == genreId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetSongsByAlbumIdAsync(int albumId)
        {
            return await _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .Where(s => s.AlbumId == albumId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetSongsByArtistIdAsync(int artistId)
        {
            return await _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .Where(s => s.ArtistId == artistId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Song>> SearchSongsAsync(string searchTerm)
        {
            return await _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .Where(s => s.Title.Contains(searchTerm) || 
                           s.User!.UserName.Contains(searchTerm) ||
                           s.Genre!.GenreName.Contains(searchTerm) ||
                           s.Album!.AlbumName.Contains(searchTerm) ||
                           s.Artist!.ArtistName.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetPopularSongsAsync(int count)
        {
            return await _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .Where(s => s.Status == "Approved")
                .OrderByDescending(s => s.LikeCount)
                .ThenByDescending(s => s.ViewCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetRecentSongsAsync(int count)
        {
            return await _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .Where(s => s.Status == "Approved")
                .OrderByDescending(s => s.UploadedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Song> AddSongAsync(Song song)
        {
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
            return song;
        }

        public async Task<Song> UpdateSongAsync(Song song)
        {
            _context.Songs.Update(song);
            await _context.SaveChangesAsync();
            return song;
        }

        public async Task DeleteSongAsync(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Songs.AnyAsync(s => s.SongId == id);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Songs.CountAsync();
        }

        public async Task ApproveSongAsync(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                song.Status = "Approved";
                song.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RejectSongAsync(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                song.Status = "Rejected";
                song.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
