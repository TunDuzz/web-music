using MediatR;

namespace WebMusic.Application.Commands.Users
{
    /// <summary>
    /// Command để xóa người dùng
    /// </summary>
    public class DeleteUserCommand : IRequest<DeleteUserResponse>
    {
        public int UserId { get; set; }
    }

    /// <summary>
    /// Response cho DeleteUserCommand
    /// </summary>
    public class DeleteUserResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
