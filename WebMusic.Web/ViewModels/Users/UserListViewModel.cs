using WebMusic.Application.DTOs;

namespace WebMusic.Web.ViewModels.Users
{
    /// <summary>
    /// ViewModel cho danh sách người dùng
    /// </summary>
    public class UserListViewModel
    {
        public IEnumerable<UserDto> Users { get; set; } = new List<UserDto>();
        public int TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public string? SearchTerm { get; set; }
    }
}
