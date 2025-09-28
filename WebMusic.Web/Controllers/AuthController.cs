using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMusic.Web.ViewModels.Auth;
using WebMusic.Application.Services;
using AuthService = WebMusic.Application.Services.IAuthenticationService;

namespace WebMusic.Web.Controllers
{
    /// <summary>
    /// Controller xử lý Authentication và Authorization
    /// </summary>
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Hiển thị trang đăng nhập
        /// </summary>
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        /// <summary>
        /// Xử lý đăng nhập
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var result = await _authService.LoginAsync(model.Email, model.Password, model.RememberMe);
                
                if (result.Success)
                {
                    _logger.LogInformation("User {Email} logged in successfully", model.Email);
                    
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message ?? "Đăng nhập thất bại");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi đăng nhập");
                return View(model);
            }
        }

        /// <summary>
        /// Hiển thị trang đăng ký
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegisterViewModel());
        }

        /// <summary>
        /// Xử lý đăng ký
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var request = new RegisterRequest
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Bio = model.Bio
                };

                var result = await _authService.RegisterAsync(request);
                
                if (result.Success)
                {
                    _logger.LogInformation("User {Email} registered successfully", model.Email);
                    TempData["SuccessMessage"] = "Đăng ký thành công! Bạn có thể đăng nhập ngay bây giờ.";
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message ?? "Đăng ký thất bại");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email: {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi đăng ký");
                return View(model);
            }
        }

        /// <summary>
        /// Đăng xuất
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            _logger.LogInformation("User logged out");
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Hiển thị trang profile
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var user = await _authService.GetCurrentUserAsync();
                if (user == null)
                {
                    return RedirectToAction(nameof(Login));
                }

                var model = new ProfileViewModel
                {
                    UserId = user.UserId,
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

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading profile");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải thông tin profile";
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Hiển thị trang chỉnh sửa profile
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            try
            {
                var user = await _authService.GetCurrentUserAsync();
                if (user == null)
                {
                    return RedirectToAction(nameof(Login));
                }

                var model = new EditProfileViewModel
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    Bio = user.Bio,
                    AvatarUrl = user.AvatarUrl
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit profile form");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải form chỉnh sửa profile";
                return RedirectToAction(nameof(Profile));
            }
        }

        /// <summary>
        /// Xử lý chỉnh sửa profile
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var request = new UpdateProfileRequest
                {
                    UserId = model.UserId,
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Bio = model.Bio,
                    AvatarUrl = model.AvatarUrl
                };

                var result = await _authService.UpdateProfileAsync(request);
                
                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Profile đã được cập nhật thành công!";
                    return RedirectToAction(nameof(Profile));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message ?? "Cập nhật profile thất bại");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile for user: {UserId}", model.UserId);
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi cập nhật profile");
                return View(model);
            }
        }

        /// <summary>
        /// Hiển thị trang đổi mật khẩu
        /// </summary>
        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        /// <summary>
        /// Xử lý đổi mật khẩu
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await _authService.GetCurrentUserAsync();
                if (user == null)
                {
                    return RedirectToAction(nameof(Login));
                }

                var request = new ChangePasswordRequest
                {
                    UserId = user.UserId,
                    CurrentPassword = model.CurrentPassword,
                    NewPassword = model.NewPassword,
                    ConfirmPassword = model.ConfirmPassword
                };

                var result = await _authService.ChangePasswordAsync(request);
                
                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công!";
                    return RedirectToAction(nameof(Profile));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message ?? "Đổi mật khẩu thất bại");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi đổi mật khẩu");
                return View(model);
            }
        }

        /// <summary>
        /// Kiểm tra quyền truy cập
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AccessDenied()
        {
            var user = await _authService.GetCurrentUserAsync();
            ViewBag.UserName = user?.UserName ?? "Unknown";
            return View();
        }
    }
}
