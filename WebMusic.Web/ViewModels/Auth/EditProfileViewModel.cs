using System.ComponentModel.DataAnnotations;

namespace WebMusic.Web.ViewModels.Auth
{
    /// <summary>
    /// ViewModel cho trang chỉnh sửa profile
    /// </summary>
    public class EditProfileViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự")]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự")]
        [Display(Name = "Tên")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ là bắt buộc")]
        [StringLength(50, ErrorMessage = "Họ không được vượt quá 50 ký tự")]
        [Display(Name = "Họ")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(500, ErrorMessage = "Tiểu sử không được vượt quá 500 ký tự")]
        [Display(Name = "Tiểu sử")]
        public string? Bio { get; set; }

        [StringLength(500, ErrorMessage = "URL avatar không được vượt quá 500 ký tự")]
        [Url(ErrorMessage = "URL avatar không hợp lệ")]
        [Display(Name = "Avatar URL")]
        public string? AvatarUrl { get; set; }
    }
}
