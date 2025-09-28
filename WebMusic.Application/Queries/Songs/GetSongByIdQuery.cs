using WebMusic.Application.DTOs;

namespace WebMusic.Application.Queries.Songs
{
    public class GetSongByIdQuery
    {
        public int SongId { get; set; }
    }

    public class GetSongByIdQueryResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public SongDto? Song { get; set; }
    }
}
