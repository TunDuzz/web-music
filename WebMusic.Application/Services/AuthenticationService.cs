using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using WebMusic.Application.DTOs;
using WebMusic.Domain.Entities;
using WebMusic.Domain.Interfaces;

namespace WebMusic.Application.Services
{
    /// <summary>
    /// Authentication Service implementation
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LoginResult> LoginAsync(string email, string password, bool rememberMe = false)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Email hoặc mật khẩu không đúng"
                    };
                }

                if (!user.IsActive)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Tài khoản đã bị khóa"
                    };
                }

                var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);
                
                if (result.Succeeded)
                {
                    // Cập nhật thời gian đăng nhập cuối
                    user.LastLoginAt = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);

                    return new LoginResult
                    {
                        Success = true,
                        User = MapToUserDto(user)
                    };
                }
                else if (result.IsLockedOut)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Tài khoản đã bị khóa do đăng nhập sai quá nhiều lần"
                    };
                }
                else
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Email hoặc mật khẩu không đúng"
                    };
                }
            }
            catch (Exception ex)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = $"Lỗi đăng nhập: {ex.Message}"
                };
            }
        }

        public async Task<RegisterResult> RegisterAsync(RegisterRequest request)
        {
            try
            {
                // Kiểm tra email đã tồn tại
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return new RegisterResult
                    {
                        Success = false,
                        Message = "Email đã được sử dụng"
                    };
                }

                // Kiểm tra username đã tồn tại
                var existingUserName = await _userManager.FindByNameAsync(request.UserName);
                if (existingUserName != null)
                {
                    return new RegisterResult
                    {
                        Success = false,
                        Message = "Tên đăng nhập đã được sử dụng"
                    };
                }

                // Tạo user mới
                var user = new ApplicationUser
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DateOfBirth = request.DateOfBirth,
                    Bio = request.Bio,
                    IsActive = true,
                    EmailConfirmed = false,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                
                if (result.Succeeded)
                {
                    // Đảm bảo role "User" tồn tại
                    await EnsureRoleExistsAsync("User");
                    
                    // Thêm role mặc định
                    await _userManager.AddToRoleAsync(user, "User");

                    return new RegisterResult
                    {
                        Success = true,
                        User = MapToUserDto(user)
                    };
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new RegisterResult
                    {
                        Success = false,
                        Message = $"Lỗi đăng ký: {errors}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RegisterResult
                {
                    Success = false,
                    Message = $"Lỗi đăng ký: {ex.Message}"
                };
            }
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<UserDto?> GetCurrentUserAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            return user != null ? MapToUserDto(user) : null;
        }

        public async Task<bool> HasPermissionAsync(string permission)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            if (user == null) return false;

            // Kiểm tra role-based permissions
            var roles = await _userManager.GetRolesAsync(user);
            
            return permission switch
            {
                "ManageUsers" => roles.Contains("Admin"),
                "ManageSongs" => roles.Contains("Admin") || roles.Contains("Moderator"),
                "ManageAlbums" => roles.Contains("Admin") || roles.Contains("Moderator"),
                "ManagePlaylists" => roles.Contains("Admin") || roles.Contains("Moderator"),
                "UploadSongs" => roles.Contains("Admin") || roles.Contains("Moderator") || roles.Contains("User"),
                _ => false
            };
        }

        public async Task<bool> IsInRoleAsync(string role)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            if (user == null) return false;

            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<UpdateProfileResult> UpdateProfileAsync(UpdateProfileRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                if (user == null)
                {
                    return new UpdateProfileResult
                    {
                        Success = false,
                        Message = "Không tìm thấy người dùng"
                    };
                }

                // Kiểm tra email đã tồn tại (trừ user hiện tại)
                if (request.Email != user.Email)
                {
                    var existingUser = await _userManager.FindByEmailAsync(request.Email);
                    if (existingUser != null)
                    {
                        return new UpdateProfileResult
                        {
                            Success = false,
                            Message = "Email đã được sử dụng"
                        };
                    }
                }

                // Kiểm tra username đã tồn tại (trừ user hiện tại)
                if (request.UserName != user.UserName)
                {
                    var existingUserName = await _userManager.FindByNameAsync(request.UserName);
                    if (existingUserName != null)
                    {
                        return new UpdateProfileResult
                        {
                            Success = false,
                            Message = "Tên đăng nhập đã được sử dụng"
                        };
                    }
                }

                // Cập nhật thông tin
                user.UserName = request.UserName;
                user.Email = request.Email;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.DateOfBirth = request.DateOfBirth;
                user.Bio = request.Bio;
                user.AvatarUrl = request.AvatarUrl;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                
                if (result.Succeeded)
                {
                    return new UpdateProfileResult
                    {
                        Success = true,
                        User = MapToUserDto(user)
                    };
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new UpdateProfileResult
                    {
                        Success = false,
                        Message = $"Lỗi cập nhật: {errors}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new UpdateProfileResult
                {
                    Success = false,
                    Message = $"Lỗi cập nhật: {ex.Message}"
                };
            }
        }

        public async Task<ChangePasswordResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                if (user == null)
                {
                    return new ChangePasswordResult
                    {
                        Success = false,
                        Message = "Không tìm thấy người dùng"
                    };
                }

                var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                
                if (result.Succeeded)
                {
                    return new ChangePasswordResult
                    {
                        Success = true
                    };
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new ChangePasswordResult
                    {
                        Success = false,
                        Message = $"Lỗi đổi mật khẩu: {errors}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ChangePasswordResult
                {
                    Success = false,
                    Message = $"Lỗi đổi mật khẩu: {ex.Message}"
                };
            }
        }

        private static UserDto MapToUserDto(ApplicationUser user)
        {
            return new UserDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Bio = user.Bio,
                AvatarUrl = user.AvatarUrl,
                Role = user.Role,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        private async Task EnsureRoleExistsAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new ApplicationRole
                {
                    Name = roleName,
                    Description = $"{roleName} role",
                    CreatedAt = DateTime.UtcNow
                };
                await _roleManager.CreateAsync(role);
            }
        }
    }
}
