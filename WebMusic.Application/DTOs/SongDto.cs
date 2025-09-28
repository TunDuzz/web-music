namespace WebMusic.Application.DTOs
{
    public class SongDto
    {
        public int SongId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string? CoverImage { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int? GenreId { get; set; }
        public string? GenreName { get; set; }
        public int? AlbumId { get; set; }
        public string? AlbumName { get; set; }
        public int? ArtistId { get; set; }
        public string? ArtistName { get; set; }
    }
}
