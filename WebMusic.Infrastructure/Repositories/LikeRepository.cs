using Microsoft.EntityFrameworkCore;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;
using WebMusic.Infrastructure.Data;

namespace WebMusic.Infrastructure.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly WebMusicDbContext _context;

        public LikeRepository(WebMusicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Like>> GetAllLikesAsync()
        {
            return await _context.Likes
                .Include(l => l.User)
                .Include(l => l.Song)
                .ToListAsync();
        }

        public async Task<Like?> GetLikeByIdAsync(int userId, int songId)
        {
            return await _context.Likes
                .Include(l => l.User)
                .Include(l => l.Song)
                .FirstOrDefaultAsync(l => l.UserId == userId && l.SongId == songId);
        }

        public async Task<IEnumerable<Like>> GetLikesBySongIdAsync(int songId)
        {
            return await _context.Likes
                .Include(l => l.User)
                .Include(l => l.Song)
                .Where(l => l.SongId == songId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Like>> GetLikesByUserIdAsync(int userId)
        {
            return await _context.Likes
                .Include(l => l.User)
                .Include(l => l.Song)
                .Where(l => l.UserId == userId)
                .ToListAsync();
        }

        public async Task<Like> AddLikeAsync(Like like)
        {
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
            return like;
        }

        public async Task DeleteLikeAsync(int userId, int songId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.SongId == songId);

            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int userId, int songId)
        {
            return await _context.Likes
                .AnyAsync(l => l.UserId == userId && l.SongId == songId);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Likes.CountAsync();
        }

        public async Task<int> GetCountBySongIdAsync(int songId)
        {
            return await _context.Likes.CountAsync(l => l.SongId == songId);
        }

        public async Task<int> GetCountByUserIdAsync(int userId)
        {
            return await _context.Likes.CountAsync(l => l.UserId == userId);
        }
    }
}
