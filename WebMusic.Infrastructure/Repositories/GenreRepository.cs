using Microsoft.EntityFrameworkCore;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;
using WebMusic.Infrastructure.Data;

namespace WebMusic.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly WebMusicDbContext _context;

        public GenreRepository(WebMusicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre?> GetGenreByIdAsync(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task<Genre?> GetGenreByNameAsync(string name)
        {
            return await _context.Genres
                .FirstOrDefaultAsync(g => g.GenreName == name);
        }

        public async Task<IEnumerable<Genre>> SearchGenresAsync(string searchTerm)
        {
            return await _context.Genres
                .Where(g => g.GenreName.Contains(searchTerm) ||
                           g.Description!.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<Genre> AddGenreAsync(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task<Genre> UpdateGenreAsync(Genre genre)
        {
            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task DeleteGenreAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Genres.AnyAsync(g => g.GenreId == id);
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await _context.Genres.AnyAsync(g => g.GenreName == name);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Genres.CountAsync();
        }
    }
}
