using System.ComponentModel.DataAnnotations;

namespace WebMusic.Domain.Entities
{
    public class Follow
    {
        public int FollowerId { get; set; }
        public ApplicationUser? Follower { get; set; }
        
        public int FollowingId { get; set; }
        public ApplicationUser? Following { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
