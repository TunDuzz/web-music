using System.ComponentModel.DataAnnotations;
using WebMusic.Application.DTOs;

namespace WebMusic.Web.ViewModels.Playlists
{
    /// <summary>
    /// ViewModel cho chỉnh sửa playlist
    /// </summary>
    public class PlaylistEditViewModel
    {
        public int PlaylistId { get; set; }

        [Required(ErrorMessage = "Tên playlist là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tên playlist không được vượt quá 200 ký tự")]
        [Display(Name = "Tên playlist")]
        public string PlaylistName { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự")]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Công khai")]
        public bool IsPublic { get; set; } = true;

        // Read-only properties
        public DateTime CreatedAt { get; set; }
        public int SongCount { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
