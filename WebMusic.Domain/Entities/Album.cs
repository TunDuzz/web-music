using System.ComponentModel.DataAnnotations;

namespace WebMusic.Domain.Entities
{
    public class Album
    {
        public int AlbumId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string AlbumName { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public DateTime? ReleaseDate { get; set; }
        
        [StringLength(500)]
        public string? CoverImage { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public int SongCount { get; set; } = 0;

        // Foreign Keys
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public int? ArtistId { get; set; }
        public Artist? Artist { get; set; }

        // Navigation Properties
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
