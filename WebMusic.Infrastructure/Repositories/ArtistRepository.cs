using Microsoft.EntityFrameworkCore;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;
using WebMusic.Infrastructure.Data;

namespace WebMusic.Infrastructure.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly WebMusicDbContext _context;

        public ArtistRepository(WebMusicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Artist>> GetAllArtistsAsync()
        {
            return await _context.Artists.ToListAsync();
        }

        public async Task<Artist?> GetArtistByIdAsync(int id)
        {
            return await _context.Artists.FindAsync(id);
        }

        public async Task<Artist?> GetArtistByNameAsync(string name)
        {
            return await _context.Artists
                .FirstOrDefaultAsync(a => a.ArtistName == name);
        }

        public async Task<IEnumerable<Artist>> SearchArtistsAsync(string searchTerm)
        {
            return await _context.Artists
                .Where(a => a.ArtistName.Contains(searchTerm) ||
                           a.Biography!.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<Artist> AddArtistAsync(Artist artist)
        {
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();
            return artist;
        }

        public async Task<Artist> UpdateArtistAsync(Artist artist)
        {
            _context.Artists.Update(artist);
            await _context.SaveChangesAsync();
            return artist;
        }

        public async Task DeleteArtistAsync(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Artists.AnyAsync(a => a.ArtistId == id);
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await _context.Artists.AnyAsync(a => a.ArtistName == name);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Artists.CountAsync();
        }
    }
}
