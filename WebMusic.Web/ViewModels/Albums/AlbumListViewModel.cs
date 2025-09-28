using WebMusic.Application.DTOs;

namespace WebMusic.Web.ViewModels.Albums
{
    /// <summary>
    /// ViewModel cho danh sách album
    /// </summary>
    public class AlbumListViewModel
    {
        public IEnumerable<AlbumDto> Albums { get; set; } = new List<AlbumDto>();
        public int TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public string? SearchTerm { get; set; }
        public int? ArtistId { get; set; }
        public int? UserId { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "desc";
    }
}
