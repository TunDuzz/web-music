namespace WebMusic.Application.DTOs
{
    public class ArtistDto
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public string? AvatarUrl { get; set; }
        public string? CoverImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public int FollowerCount { get; set; }
    }
}
