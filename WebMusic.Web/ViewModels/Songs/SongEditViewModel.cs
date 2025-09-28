using System.ComponentModel.DataAnnotations;
using WebMusic.Application.DTOs;

namespace WebMusic.Web.ViewModels.Songs
{
    /// <summary>
    /// ViewModel cho chỉnh sửa bài hát
    /// </summary>
    public class SongEditViewModel
    {
        public int SongId { get; set; }

        [Required(ErrorMessage = "Tên bài hát là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tên bài hát không được vượt quá 200 ký tự")]
        [Display(Name = "Tên bài hát")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "URL ảnh bìa không được vượt quá 500 ký tự")]
        [Url(ErrorMessage = "URL ảnh bìa không hợp lệ")]
        [Display(Name = "Ảnh bìa")]
        public string? CoverImage { get; set; }

        [Display(Name = "Thể loại")]
        public int? GenreId { get; set; }

        [Display(Name = "Album")]
        public int? AlbumId { get; set; }

        [Display(Name = "Nghệ sĩ")]
        public int? ArtistId { get; set; }

        // Dropdown lists
        public IEnumerable<GenreDto>? Genres { get; set; }
        public IEnumerable<AlbumDto>? Albums { get; set; }
        public IEnumerable<ArtistDto>? Artists { get; set; }

        // Read-only properties
        public string FileUrl { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
