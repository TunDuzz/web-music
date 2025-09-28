using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebMusic.Domain.Entities
{
    /// <summary>
    /// Application User class kế thừa từ IdentityUser
    /// </summary>
    public class ApplicationUser : IdentityUser<int>
    {
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        public DateTime? DateOfBirth { get; set; }
        
        [StringLength(500)]
        public string? Bio { get; set; }
        
        [StringLength(500)]
        public string? AvatarUrl { get; set; }
        
        [StringLength(20)]
        public string Role { get; set; } = "User"; // User / Admin / Moderator
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLoginAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public ICollection<Song> Songs { get; set; } = new List<Song>();
        public ICollection<Album> Albums { get; set; } = new List<Album>();
        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<PlayHistory> PlayHistories { get; set; } = new List<PlayHistory>();
        public ICollection<Follow> Followers { get; set; } = new List<Follow>();
        public ICollection<Follow> Following { get; set; } = new List<Follow>();
    }
}
