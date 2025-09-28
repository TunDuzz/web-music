namespace WebMusic.Application.DTOs
{
    public class PlaylistDto
    {
        public int PlaylistId { get; set; }
        public string PlaylistName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsPublic { get; set; }
        public int SongCount { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public List<SongDto>? Songs { get; set; }
    }
}
