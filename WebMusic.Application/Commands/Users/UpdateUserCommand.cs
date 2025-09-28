using MediatR;
using WebMusic.Application.DTOs;

namespace WebMusic.Application.Commands.Users
{
    /// <summary>
    /// Command để cập nhật người dùng
    /// </summary>
    public class UpdateUserCommand : IRequest<UpdateUserResponse>
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// Response cho UpdateUserCommand
    /// </summary>
    public class UpdateUserResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public UserDto? User { get; set; }
    }
}
