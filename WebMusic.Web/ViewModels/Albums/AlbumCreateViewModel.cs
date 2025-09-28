using System.ComponentModel.DataAnnotations;
using WebMusic.Application.DTOs;

namespace WebMusic.Web.ViewModels.Albums
{
    /// <summary>
    /// ViewModel cho tạo album mới
    /// </summary>
    public class AlbumCreateViewModel
    {
        [Required(ErrorMessage = "Tên album là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tên album không được vượt quá 200 ký tự")]
        [Display(Name = "Tên album")]
        public string AlbumName { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự")]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Ngày phát hành")]
        public DateTime? ReleaseDate { get; set; }

        [StringLength(500, ErrorMessage = "URL ảnh bìa không được vượt quá 500 ký tự")]
        [Url(ErrorMessage = "URL ảnh bìa không hợp lệ")]
        [Display(Name = "Ảnh bìa")]
        public string? CoverImage { get; set; }

        [Display(Name = "Nghệ sĩ")]
        public int? ArtistId { get; set; }

        // Dropdown lists
        public IEnumerable<ArtistDto>? Artists { get; set; }
    }
}
