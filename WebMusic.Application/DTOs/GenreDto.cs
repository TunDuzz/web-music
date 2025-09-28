namespace WebMusic.Application.DTOs
{
    public class GenreDto
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CoverImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SongCount { get; set; }
    }
}
