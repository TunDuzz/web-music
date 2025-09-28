using WebMusic.Application.DTOs;

namespace WebMusic.Application.Services
{
    /// <summary>
    /// Interface cho Authentication Service
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Đăng nhập người dùng
        /// </summary>
        Task<LoginResult> LoginAsync(string email, string password, bool rememberMe = false);
        
        /// <summary>
        /// Đăng ký người dùng mới
        /// </summary>
        Task<RegisterResult> RegisterAsync(RegisterRequest request);
        
        /// <summary>
        /// Đăng xuất người dùng
        /// </summary>
        Task LogoutAsync();
        
        /// <summary>
        /// Lấy thông tin người dùng hiện tại
        /// </summary>
        Task<UserDto?> GetCurrentUserAsync();
        
        /// <summary>
        /// Kiểm tra quyền của người dùng
        /// </summary>
        Task<bool> HasPermissionAsync(string permission);
        
        /// <summary>
        /// Kiểm tra role của người dùng
        /// </summary>
        Task<bool> IsInRoleAsync(string role);
        
        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        Task<UpdateProfileResult> UpdateProfileAsync(UpdateProfileRequest request);
        
        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        Task<ChangePasswordResult> ChangePasswordAsync(ChangePasswordRequest request);
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public UserDto? User { get; set; }
    }

    public class RegisterResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public UserDto? User { get; set; }
    }

    public class UpdateProfileResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public UserDto? User { get; set; }
    }

    public class ChangePasswordResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class RegisterRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Bio { get; set; }
    }

    public class UpdateProfileRequest
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
    }

    public class ChangePasswordRequest
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
