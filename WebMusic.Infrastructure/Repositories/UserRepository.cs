using Microsoft.EntityFrameworkCore;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;
using WebMusic.Infrastructure.Data;

namespace WebMusic.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly WebMusicDbContext _context;

        public UserRepository(WebMusicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(MapToUser);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user != null ? MapToUser(user) : null;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user != null ? MapToUser(user) : null;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == username);
            return user != null ? MapToUser(user) : null;
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
        {
            var users = await _context.Users
                .Where(u => u.UserName.Contains(searchTerm) || u.Email.Contains(searchTerm))
                .ToListAsync();
            return users.Select(MapToUser);
        }

        public async Task<User> AddUserAsync(User user)
        {
            var appUser = MapToApplicationUser(user);
            _context.Users.Add(appUser);
            await _context.SaveChangesAsync();
            return MapToUser(appUser);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var appUser = await _context.Users.FindAsync(user.UserId);
            if (appUser != null)
            {
                UpdateApplicationUser(appUser, user);
                _context.Users.Update(appUser);
                await _context.SaveChangesAsync();
            }
            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetUsersAsync(string? searchTerm, int page, int pageSize, string? sortBy, string? sortDirection)
        {
            var query = _context.Users.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => 
                    u.UserName.Contains(searchTerm) ||
                    u.Email.Contains(searchTerm) ||
                    u.FirstName.Contains(searchTerm) ||
                    u.LastName.Contains(searchTerm));
            }

            // Apply sorting
            query = sortBy?.ToLower() switch
            {
                "username" => sortDirection == "desc" ? query.OrderByDescending(u => u.UserName) : query.OrderBy(u => u.UserName),
                "email" => sortDirection == "desc" ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                "firstname" => sortDirection == "desc" ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),
                "lastname" => sortDirection == "desc" ? query.OrderByDescending(u => u.LastName) : query.OrderBy(u => u.LastName),
                "createdat" => sortDirection == "desc" ? query.OrderByDescending(u => u.CreatedAt) : query.OrderBy(u => u.CreatedAt),
                _ => query.OrderBy(u => u.UserName)
            };

            // Apply pagination
            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return users.Select(MapToUser);
        }

        public async Task<int> GetUsersCountAsync(string? searchTerm)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => 
                    u.UserName.Contains(searchTerm) ||
                    u.Email.Contains(searchTerm) ||
                    u.FirstName.Contains(searchTerm) ||
                    u.LastName.Contains(searchTerm));
            }

            return await query.CountAsync();
        }

        private static User MapToUser(ApplicationUser appUser)
        {
            return new User
            {
                UserId = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                DateOfBirth = appUser.DateOfBirth,
                Bio = appUser.Bio,
                AvatarUrl = appUser.AvatarUrl,
                Role = appUser.Role,
                IsActive = appUser.IsActive,
                EmailConfirmed = appUser.EmailConfirmed,
                CreatedAt = appUser.CreatedAt,
                LastLoginAt = appUser.LastLoginAt,
                UpdatedAt = appUser.UpdatedAt,
                PasswordHash = appUser.PasswordHash
            };
        }

        private static ApplicationUser MapToApplicationUser(User user)
        {
            return new ApplicationUser
            {
                Id = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Bio = user.Bio,
                AvatarUrl = user.AvatarUrl,
                Role = user.Role,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                UpdatedAt = user.UpdatedAt,
                PasswordHash = user.PasswordHash
            };
        }

        private static void UpdateApplicationUser(ApplicationUser appUser, User user)
        {
            appUser.UserName = user.UserName;
            appUser.Email = user.Email;
            appUser.FirstName = user.FirstName;
            appUser.LastName = user.LastName;
            appUser.DateOfBirth = user.DateOfBirth;
            appUser.Bio = user.Bio;
            appUser.AvatarUrl = user.AvatarUrl;
            appUser.Role = user.Role;
            appUser.IsActive = user.IsActive;
            appUser.EmailConfirmed = user.EmailConfirmed;
            appUser.UpdatedAt = user.UpdatedAt;
        }
    }
}