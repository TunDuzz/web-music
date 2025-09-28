using WebMusic.Application.DTOs;

namespace WebMusic.Application.Commands.Songs
{
    public class UpdateSongCommand
    {
        public int SongId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? CoverImage { get; set; }
        public int? GenreId { get; set; }
        public int? AlbumId { get; set; }
        public int? ArtistId { get; set; }
    }

    public class UpdateSongCommandResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public SongDto? Song { get; set; }
    }
}
