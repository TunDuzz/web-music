using WebMusic.Domain.Entities;

namespace WebMusic.Domain.Interfaces
{
    public interface ILikeRepository
    {
        Task<IEnumerable<Like>> GetAllLikesAsync();
        Task<Like?> GetLikeByIdAsync(int userId, int songId);
        Task<IEnumerable<Like>> GetLikesBySongIdAsync(int songId);
        Task<IEnumerable<Like>> GetLikesByUserIdAsync(int userId);
        Task<Like> AddLikeAsync(Like like);
        Task DeleteLikeAsync(int userId, int songId);
        Task<bool> ExistsAsync(int userId, int songId);
        Task<int> GetTotalCountAsync();
        Task<int> GetCountBySongIdAsync(int songId);
        Task<int> GetCountByUserIdAsync(int userId);
    }
}
