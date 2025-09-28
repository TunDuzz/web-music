using MediatR;
using WebMusic.Application.DTOs;

namespace WebMusic.Application.Queries.Users
{
    /// <summary>
    /// Query để lấy thông tin người dùng theo ID
    /// </summary>
    public class GetUserByIdQuery : IRequest<GetUserByIdResponse>
    {
        public int UserId { get; set; }
    }

    /// <summary>
    /// Response cho GetUserByIdQuery
    /// </summary>
    public class GetUserByIdResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public UserDto? User { get; set; }
    }
}
