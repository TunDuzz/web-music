using WebMusic.Application.DTOs;

namespace WebMusic.Application.Commands.Albums
{
    public class UpdateAlbumCommand
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? CoverImage { get; set; }
        public int? ArtistId { get; set; }
    }

    public class UpdateAlbumCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public AlbumDto? Album { get; set; }
    }
}
