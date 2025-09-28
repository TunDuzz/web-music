namespace WebMusic.Application.DTOs
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsEdited { get; set; }
        public int LikeCount { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? AvatarUrl { get; set; }
        public int SongId { get; set; }
    }
}
