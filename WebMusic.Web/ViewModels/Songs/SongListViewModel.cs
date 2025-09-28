using WebMusic.Application.DTOs;

namespace WebMusic.Web.ViewModels.Songs
{
    /// <summary>
    /// ViewModel cho danh sách bài hát
    /// </summary>
    public class SongListViewModel
    {
        public IEnumerable<SongDto> Songs { get; set; } = new List<SongDto>();
        public int TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public string? SearchTerm { get; set; }
        public int? GenreId { get; set; }
        public int? AlbumId { get; set; }
        public int? ArtistId { get; set; }
        public int? UserId { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "desc";
    }
}
