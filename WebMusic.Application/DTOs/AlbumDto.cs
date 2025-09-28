namespace WebMusic.Application.DTOs
{
    public class AlbumDto
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? CoverImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int SongCount { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int? ArtistId { get; set; }
        public string? ArtistName { get; set; }
    }
}
