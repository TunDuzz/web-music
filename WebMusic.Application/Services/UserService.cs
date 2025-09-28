using WebMusic.Application.Commands.Users;
using WebMusic.Application.DTOs;
using WebMusic.Application.Queries.Users;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;
using MediatR;

namespace WebMusic.Application.Services
{
    public interface IUserService
    {
        Task<GetUsersResponse> GetUsersAsync(GetUsersQuery query);
        Task<GetUserByIdResponse> GetUserByIdAsync(GetUserByIdQuery query);
        Task<CreateUserResponse> CreateUserAsync(CreateUserCommand command);
        Task<UpdateUserResponse> UpdateUserAsync(UpdateUserCommand command);
        Task<DeleteUserResponse> DeleteUserAsync(DeleteUserCommand command);
        
        // Legacy methods for backward compatibility
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<List<UserDto>> SearchUsersAsync(string searchTerm);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user != null ? MapToUserDto(user) : null;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return user != null ? MapToUserDto(user) : null;
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            return user != null ? MapToUserDto(user) : null;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(MapToUserDto).ToList();
        }

        public async Task<List<UserDto>> SearchUsersAsync(string searchTerm)
        {
            var users = await _userRepository.SearchUsersAsync(searchTerm);
            return users.Select(MapToUserDto).ToList();
        }

        public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
        {
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = request.Role ?? "User"
            };

            var createdUser = await _userRepository.AddUserAsync(user);
            return MapToUserDto(createdUser);
        }

        public async Task<UserDto> UpdateUserAsync(UpdateUserRequest request)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(request.UserId);
            if (existingUser == null)
                throw new ArgumentException("User not found");

            existingUser.UserName = request.UserName;
            existingUser.Email = request.Email;
            existingUser.FirstName = request.FirstName;
            existingUser.LastName = request.LastName;
            existingUser.AvatarUrl = request.AvatarUrl;
            existingUser.UpdatedAt = DateTime.UtcNow;

            var updatedUser = await _userRepository.UpdateUserAsync(existingUser);
            return MapToUserDto(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return false;

            await _userRepository.DeleteUserAsync(id);
            return true;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepository.EmailExistsAsync(email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _userRepository.UsernameExistsAsync(username);
        }

        private static UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Bio = user.Bio,
                Role = user.Role,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        // New CQRS methods
        public async Task<GetUsersResponse> GetUsersAsync(GetUsersQuery query)
        {
            try
            {
                var users = await _userRepository.GetUsersAsync(
                    query.SearchTerm, 
                    query.Page, 
                    query.PageSize, 
                    query.SortBy, 
                    query.SortDirection);

                var totalCount = await _userRepository.GetUsersCountAsync(query.SearchTerm);

                return new GetUsersResponse
                {
                    Success = true,
                    Users = users.Select(MapToUserDto),
                    TotalCount = totalCount
                };
            }
            catch (Exception ex)
            {
                return new GetUsersResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<GetUserByIdResponse> GetUserByIdAsync(GetUserByIdQuery query)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(query.UserId);
                if (user == null)
                {
                    return new GetUserByIdResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                return new GetUserByIdResponse
                {
                    Success = true,
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                return new GetUserByIdResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<CreateUserResponse> CreateUserAsync(CreateUserCommand command)
        {
            try
            {
                // Check if email already exists
                if (await EmailExistsAsync(command.Email))
                {
                    return new CreateUserResponse
                    {
                        Success = false,
                        Message = "Email already exists"
                    };
                }


                var user = new User
                {
                    UserName = command.UserName,
                    Email = command.Email,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    DateOfBirth = command.DateOfBirth,
                    Bio = command.Bio,
                    IsActive = true,
                    EmailConfirmed = false,
                    CreatedAt = DateTime.UtcNow
                };

                await _userRepository.AddUserAsync(user);

                return new CreateUserResponse
                {
                    Success = true,
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                return new CreateUserResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserCommand command)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(command.UserId);
                if (user == null)
                {
                    return new UpdateUserResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                // Check if email already exists (excluding current user)
                if (command.Email != user.Email && await EmailExistsAsync(command.Email))
                {
                    return new UpdateUserResponse
                    {
                        Success = false,
                        Message = "Email already exists"
                    };
                }

                    user.UserName = command.UserName;
                user.Email = command.Email;
                user.FirstName = command.FirstName;
                user.LastName = command.LastName;
                user.DateOfBirth = command.DateOfBirth;
                user.Bio = command.Bio;
                user.IsActive = command.IsActive;
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.UpdateUserAsync(user);

                return new UpdateUserResponse
                {
                    Success = true,
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                return new UpdateUserResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<DeleteUserResponse> DeleteUserAsync(DeleteUserCommand command)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(command.UserId);
                if (user == null)
                {
                    return new DeleteUserResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                await _userRepository.DeleteUserAsync(command.UserId);

                return new DeleteUserResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new DeleteUserResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }

    public class CreateUserRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Role { get; set; }
    }

    public class UpdateUserRequest
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
    }
}
