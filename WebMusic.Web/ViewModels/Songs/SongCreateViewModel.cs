using System.ComponentModel.DataAnnotations;
using WebMusic.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace WebMusic.Web.ViewModels.Songs
{
    /// <summary>
    /// ViewModel cho tạo bài hát mới
    /// </summary>
    public class SongCreateViewModel
    {
        [Required(ErrorMessage = "Tên bài hát là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tên bài hát không được vượt quá 200 ký tự")]
        [Display(Name = "Tên bài hát")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "File nhạc")]
        public IFormFile? AudioFile { get; set; }

        [StringLength(500, ErrorMessage = "URL file nhạc không được vượt quá 500 ký tự")]
        [Url(ErrorMessage = "URL không hợp lệ")]
        [Display(Name = "URL file nhạc (tùy chọn)")]
        public string? FileUrl { get; set; }

        [Display(Name = "Ảnh bìa")]
        public IFormFile? CoverImageFile { get; set; }

        [StringLength(500, ErrorMessage = "URL ảnh bìa không được vượt quá 500 ký tự")]
        [Url(ErrorMessage = "URL ảnh bìa không hợp lệ")]
        [Display(Name = "URL ảnh bìa (tùy chọn)")]
        public string? CoverImage { get; set; }

        [Required(ErrorMessage = "Thời lượng là bắt buộc")]
        [Range(1, 3600, ErrorMessage = "Thời lượng phải từ 1 đến 3600 giây")]
        [Display(Name = "Thời lượng (giây)")]
        public int Duration { get; set; }

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
    }
}
