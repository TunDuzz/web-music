using WebMusic.Application.DTOs;

namespace WebMusic.Application.Queries.Albums
{
    public class GetAlbumByIdQuery
    {
        public int AlbumId { get; set; }
    }

    public class GetAlbumByIdQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public AlbumDto? Album { get; set; }
    }
}
