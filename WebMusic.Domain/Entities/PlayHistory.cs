using System.ComponentModel.DataAnnotations;

namespace WebMusic.Domain.Entities
{
    public class PlayHistory
    {
        public int PlayHistoryId { get; set; }
        
        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
        
        public int Duration { get; set; } // Duration played in seconds
        
        public bool IsCompleted { get; set; } = false;

        // Foreign Keys
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public int SongId { get; set; }
        public Song? Song { get; set; }
    }
}
