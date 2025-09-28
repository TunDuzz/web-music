using System.ComponentModel.DataAnnotations;

namespace WebMusic.Domain.Entities
{
    public class Like
    {
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }
        
        public int SongId { get; set; }
        public Song? Song { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
