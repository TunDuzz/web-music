using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebMusic.Domain.Entities
{
    /// <summary>
    /// Application Role class kế thừa từ IdentityRole
    /// </summary>
    public class ApplicationRole : IdentityRole<int>
    {
        [StringLength(500)]
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
