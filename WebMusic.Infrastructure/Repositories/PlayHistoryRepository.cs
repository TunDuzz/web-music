using Microsoft.EntityFrameworkCore;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;
using WebMusic.Infrastructure.Data;

namespace WebMusic.Infrastructure.Repositories
{
    public class PlayHistoryRepository : IPlayHistoryRepository
    {
        private readonly WebMusicDbContext _context;

        public PlayHistoryRepository(WebMusicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlayHistory>> GetAllPlayHistoriesAsync()
        {
            return await _context.PlayHistories
                .Include(ph => ph.User)
                .Include(ph => ph.Song)
                .ToListAsync();
        }

        public async Task<PlayHistory?> GetPlayHistoryByIdAsync(int id)
        {
            return await _context.PlayHistories
                .Include(ph => ph.User)
                .Include(ph => ph.Song)
                .FirstOrDefaultAsync(ph => ph.PlayHistoryId == id);
        }

        public async Task<IEnumerable<PlayHistory>> GetPlayHistoriesByUserIdAsync(int userId)
        {
            return await _context.PlayHistories
                .Include(ph => ph.User)
                .Include(ph => ph.Song)
                .Where(ph => ph.UserId == userId)
                .OrderByDescending(ph => ph.PlayedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlayHistory>> GetPlayHistoriesBySongIdAsync(int songId)
        {
            return await _context.PlayHistories
                .Include(ph => ph.User)
                .Include(ph => ph.Song)
                .Where(ph => ph.SongId == songId)
                .OrderByDescending(ph => ph.PlayedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlayHistory>> GetRecentPlayHistoriesAsync(int userId, int count)
        {
            return await _context.PlayHistories
                .Include(ph => ph.User)
                .Include(ph => ph.Song)
                .Where(ph => ph.UserId == userId)
                .OrderByDescending(ph => ph.PlayedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<PlayHistory> AddPlayHistoryAsync(PlayHistory playHistory)
        {
            _context.PlayHistories.Add(playHistory);
            await _context.SaveChangesAsync();
            return playHistory;
        }

        public async Task<PlayHistory> UpdatePlayHistoryAsync(PlayHistory playHistory)
        {
            _context.PlayHistories.Update(playHistory);
            await _context.SaveChangesAsync();
            return playHistory;
        }

        public async Task DeletePlayHistoryAsync(int id)
        {
            var playHistory = await _context.PlayHistories.FindAsync(id);
            if (playHistory != null)
            {
                _context.PlayHistories.Remove(playHistory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PlayHistories.AnyAsync(ph => ph.PlayHistoryId == id);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.PlayHistories.CountAsync();
        }

        public async Task<int> GetCountByUserIdAsync(int userId)
        {
            return await _context.PlayHistories.CountAsync(ph => ph.UserId == userId);
        }

        public async Task<int> GetCountBySongIdAsync(int songId)
        {
            return await _context.PlayHistories.CountAsync(ph => ph.SongId == songId);
        }
    }
}
