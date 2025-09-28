using MediatR;
using WebMusic.Application.DTOs;

namespace WebMusic.Application.Queries.Users
{
    /// <summary>
    /// Query để lấy danh sách người dùng
    /// </summary>
    public class GetUsersQuery : IRequest<GetUsersResponse>
    {
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "asc";
    }

    /// <summary>
    /// Response cho GetUsersQuery
    /// </summary>
    public class GetUsersResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public IEnumerable<UserDto> Users { get; set; } = new List<UserDto>();
        public int TotalCount { get; set; }
    }
}
