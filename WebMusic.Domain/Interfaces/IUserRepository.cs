using WebMusic.Domain.Entities;

namespace WebMusic.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task<int> GetTotalCountAsync();
        Task UpdateLastLoginAsync(int userId);
        Task<IEnumerable<User>> GetUsersAsync(string? searchTerm, int page, int pageSize, string? sortBy, string? sortDirection);
        Task<int> GetUsersCountAsync(string? searchTerm);
    }
}
