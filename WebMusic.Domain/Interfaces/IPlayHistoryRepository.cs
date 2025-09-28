using WebMusic.Domain.Entities;

namespace WebMusic.Domain.Interfaces
{
    public interface IPlayHistoryRepository
    {
        Task<IEnumerable<PlayHistory>> GetAllPlayHistoriesAsync();
        Task<PlayHistory?> GetPlayHistoryByIdAsync(int id);
        Task<IEnumerable<PlayHistory>> GetPlayHistoriesByUserIdAsync(int userId);
        Task<IEnumerable<PlayHistory>> GetPlayHistoriesBySongIdAsync(int songId);
        Task<IEnumerable<PlayHistory>> GetRecentPlayHistoriesAsync(int userId, int count);
        Task<PlayHistory> AddPlayHistoryAsync(PlayHistory playHistory);
        Task<PlayHistory> UpdatePlayHistoryAsync(PlayHistory playHistory);
        Task DeletePlayHistoryAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetTotalCountAsync();
        Task<int> GetCountByUserIdAsync(int userId);
        Task<int> GetCountBySongIdAsync(int songId);
    }
}
