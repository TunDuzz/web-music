using WebMusic.Application.DTOs;

namespace WebMusic.Application.Commands.Songs
{
    public class CreateSongCommand
    {
        public string Title { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string? CoverImage { get; set; }
        public int Duration { get; set; }
        public int UserId { get; set; }
        public int? GenreId { get; set; }
        public int? AlbumId { get; set; }
        public int? ArtistId { get; set; }
    }

    public class CreateSongCommandResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public SongDto? Song { get; set; }
    }
}
