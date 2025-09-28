using WebMusic.Application.DTOs;

namespace WebMusic.Application.Queries.Albums
{
    public class GetAlbumsQuery
    {
        public int? UserId { get; set; }
        public int? ArtistId { get; set; }
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "desc";
    }

    public class GetAlbumsQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<AlbumDto> Albums { get; set; } = new List<AlbumDto>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
