using WebMusic.Domain.Entities;

namespace WebMusic.Domain.Interfaces
{
    public interface IFollowRepository
    {
        Task<IEnumerable<Follow>> GetAllFollowsAsync();
        Task<Follow?> GetFollowByIdAsync(int followerId, int followingId);
        Task<IEnumerable<Follow>> GetFollowersAsync(int userId);
        Task<IEnumerable<Follow>> GetFollowingAsync(int userId);
        Task<Follow> AddFollowAsync(Follow follow);
        Task DeleteFollowAsync(int followerId, int followingId);
        Task<bool> ExistsAsync(int followerId, int followingId);
        Task<int> GetTotalCountAsync();
        Task<int> GetFollowerCountAsync(int userId);
        Task<int> GetFollowingCountAsync(int userId);
    }
}
