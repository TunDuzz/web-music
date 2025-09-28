using System.ComponentModel.DataAnnotations;

namespace WebMusic.Domain.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        
        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public bool IsEdited { get; set; } = false;
        
        public int LikeCount { get; set; } = 0;

        // Foreign Keys
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public int SongId { get; set; }
        public Song? Song { get; set; }
    }
}
