using System.ComponentModel.DataAnnotations;

namespace WebMusic.Domain.Entities
{
    public class Genre
    {
        public int GenreId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string GenreName { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [StringLength(500)]
        public string? CoverImage { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public int SongCount { get; set; } = 0;

        // Navigation Properties
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
