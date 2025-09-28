using Microsoft.EntityFrameworkCore;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;
using WebMusic.Infrastructure.Data;

namespace WebMusic.Infrastructure.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly WebMusicDbContext _context;

        public AlbumRepository(WebMusicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
        {
            return await _context.Albums
                .Include(a => a.User)
                .Include(a => a.Artist)
                .ToListAsync();
        }

        public async Task<Album?> GetAlbumByIdAsync(int id)
        {
            return await _context.Albums
                .Include(a => a.User)
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(a => a.AlbumId == id);
        }

        public async Task<IEnumerable<Album>> GetAlbumsByUserIdAsync(int userId)
        {
            return await _context.Albums
                .Include(a => a.User)
                .Include(a => a.Artist)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Album>> GetAlbumsByArtistIdAsync(int artistId)
        {
            return await _context.Albums
                .Include(a => a.User)
                .Include(a => a.Artist)
                .Where(a => a.ArtistId == artistId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Album>> SearchAlbumsAsync(string searchTerm)
        {
            return await _context.Albums
                .Include(a => a.User)
                .Include(a => a.Artist)
                .Where(a => a.AlbumName.Contains(searchTerm) ||
                           a.Description!.Contains(searchTerm) ||
                           a.User!.UserName.Contains(searchTerm) ||
                           a.Artist!.ArtistName.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<Album> AddAlbumAsync(Album album)
        {
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();
            return album;
        }

        public async Task<Album> UpdateAlbumAsync(Album album)
        {
            _context.Albums.Update(album);
            await _context.SaveChangesAsync();
            return album;
        }

        public async Task DeleteAlbumAsync(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Albums.AnyAsync(a => a.AlbumId == id);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Albums.CountAsync();
        }
    }
}
