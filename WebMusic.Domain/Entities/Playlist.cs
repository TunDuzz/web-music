using System.ComponentModel.DataAnnotations;

namespace WebMusic.Domain.Entities
{
    public class Playlist
    {
        public int PlaylistId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string PlaylistName { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public bool IsPublic { get; set; } = true;
        
        public int SongCount { get; set; } = 0;

        // Foreign Keys
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        // Navigation Properties
        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
    }

    public class PlaylistSong
    {
        public int PlaylistId { get; set; }
        public Playlist? Playlist { get; set; }
        
        public int SongId { get; set; }
        public Song? Song { get; set; }
        
        public int Order { get; set; } = 0;
        
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
