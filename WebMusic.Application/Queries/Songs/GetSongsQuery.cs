using WebMusic.Application.DTOs;

namespace WebMusic.Application.Queries.Songs
{
    public class GetSongsQuery
    {
        public int? UserId { get; set; }
        public int? GenreId { get; set; }
        public int? AlbumId { get; set; }
        public int? ArtistId { get; set; }
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "desc";
    }

    public class GetSongsQueryResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<SongDto> Songs { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
