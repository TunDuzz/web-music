using System.ComponentModel.DataAnnotations;

namespace WebMusic.Domain.Entities
{
    public class Artist
    {
        public int ArtistId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ArtistName { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Biography { get; set; }
        
        [StringLength(500)]
        public string? AvatarUrl { get; set; }
        
        [StringLength(500)]
        public string? CoverImage { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int FollowerCount { get; set; } = 0;

        // Navigation Properties
        public ICollection<Song> Songs { get; set; } = new List<Song>();
        public ICollection<Album> Albums { get; set; } = new List<Album>();
    }
}
