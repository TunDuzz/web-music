using System.ComponentModel.DataAnnotations;

namespace WebMusic.Domain.Entities
{
    public class Song
    {
        public int SongId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string FileUrl { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? CoverImage { get; set; }
        
        [Required]
        public int Duration { get; set; } // Duration in seconds
        
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending / Approved / Rejected
        
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public int ViewCount { get; set; } = 0;
        
        public int LikeCount { get; set; } = 0;
        
        public int CommentCount { get; set; } = 0;

        // Foreign Keys
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public int? GenreId { get; set; }
        public Genre? Genre { get; set; }

        public int? AlbumId { get; set; }
        public Album? Album { get; set; }

        public int? ArtistId { get; set; }
        public Artist? Artist { get; set; }

        // Navigation Properties
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
        public ICollection<PlayHistory> PlayHistories { get; set; } = new List<PlayHistory>();
    }
}
