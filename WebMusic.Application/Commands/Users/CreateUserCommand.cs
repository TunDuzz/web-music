using MediatR;
using WebMusic.Application.DTOs;

namespace WebMusic.Application.Commands.Users
{
    /// <summary>
    /// Command để tạo người dùng mới
    /// </summary>
    public class CreateUserCommand : IRequest<CreateUserResponse>
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Bio { get; set; }
    }

    /// <summary>
    /// Response cho CreateUserCommand
    /// </summary>
    public class CreateUserResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public UserDto? User { get; set; }
    }
}
