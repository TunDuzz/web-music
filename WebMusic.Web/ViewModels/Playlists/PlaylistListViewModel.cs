using WebMusic.Application.DTOs;

namespace WebMusic.Web.ViewModels.Playlists
{
    /// <summary>
    /// ViewModel cho danh sách playlist
    /// </summary>
    public class PlaylistListViewModel
    {
        public IEnumerable<PlaylistDto> Playlists { get; set; } = new List<PlaylistDto>();
        public int TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public string? SearchTerm { get; set; }
        public int? UserId { get; set; }
        public bool? IsPublic { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "desc";
    }
}
