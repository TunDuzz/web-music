using Microsoft.EntityFrameworkCore;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;
using WebMusic.Infrastructure.Data;

namespace WebMusic.Infrastructure.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        private readonly WebMusicDbContext _context;

        public FollowRepository(WebMusicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Follow>> GetAllFollowsAsync()
        {
            return await _context.Follows
                .Include(f => f.Follower)
                .Include(f => f.Following)
                .ToListAsync();
        }

        public async Task<Follow?> GetFollowByIdAsync(int followerId, int followingId)
        {
            return await _context.Follows
                .Include(f => f.Follower)
                .Include(f => f.Following)
                .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
        }

        public async Task<IEnumerable<Follow>> GetFollowersAsync(int userId)
        {
            return await _context.Follows
                .Include(f => f.Follower)
                .Include(f => f.Following)
                .Where(f => f.FollowingId == userId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Follow>> GetFollowingAsync(int userId)
        {
            return await _context.Follows
                .Include(f => f.Follower)
                .Include(f => f.Following)
                .Where(f => f.FollowerId == userId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<Follow> AddFollowAsync(Follow follow)
        {
            _context.Follows.Add(follow);
            await _context.SaveChangesAsync();
            return follow;
        }

        public async Task DeleteFollowAsync(int followerId, int followingId)
        {
            var follow = await _context.Follows
                .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

            if (follow != null)
            {
                _context.Follows.Remove(follow);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int followerId, int followingId)
        {
            return await _context.Follows
                .AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Follows.CountAsync();
        }

        public async Task<int> GetFollowerCountAsync(int userId)
        {
            return await _context.Follows.CountAsync(f => f.FollowingId == userId);
        }

        public async Task<int> GetFollowingCountAsync(int userId)
        {
            return await _context.Follows.CountAsync(f => f.FollowerId == userId);
        }
    }
}
