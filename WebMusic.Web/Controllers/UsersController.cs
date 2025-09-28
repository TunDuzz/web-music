using Microsoft.AspNetCore.Mvc;
using WebMusic.Application.Services;
using WebMusic.Web.Services;
using WebMusic.Web.ViewModels.Users;

namespace WebMusic.Web.Controllers
{
    /// <summary>
    /// Controller xử lý quản lý người dùng
    /// </summary>
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Hiển thị danh sách người dùng
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm</param>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng mỗi trang</param>
        /// <returns>View danh sách người dùng</returns>
        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, int page = 1, int pageSize = 10)
        {
            try
            {
                var query = new Application.Queries.Users.GetUsersQuery
                {
                    SearchTerm = searchTerm,
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _userService.GetUsersAsync(query);
                
                var model = new UserListViewModel
                {
                    Users = result.Users,
                    TotalCount = result.TotalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize),
                    SearchTerm = searchTerm
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading users list");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải danh sách người dùng";
                return View(new UserListViewModel());
            }
        }

        /// <summary>
        /// Hiển thị chi tiết người dùng
        /// </summary>
        /// <param name="id">ID người dùng</param>
        /// <returns>View chi tiết người dùng</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var query = new Application.Queries.Users.GetUserByIdQuery { UserId = id };
                var result = await _userService.GetUserByIdAsync(query);

                if (!result.Success || result.User == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy người dùng";
                    return RedirectToAction(nameof(Index));
                }

                return View(result.User);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user details for ID: {UserId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải chi tiết người dùng";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Hiển thị form tạo người dùng mới
        /// </summary>
        /// <returns>View form tạo người dùng</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserCreateViewModel());
        }

        /// <summary>
        /// Xử lý tạo người dùng mới
        /// </summary>
        /// <param name="model">Dữ liệu người dùng</param>
        /// <returns>Redirect về danh sách hoặc hiển thị lỗi</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var command = new Application.Commands.Users.CreateUserCommand
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Bio = model.Bio
                };

                var result = await _userService.CreateUserAsync(command);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Tạo người dùng thành công";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message ?? "Có lỗi xảy ra khi tạo người dùng";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo người dùng";
                return View(model);
            }
        }

        /// <summary>
        /// Hiển thị form chỉnh sửa người dùng
        /// </summary>
        /// <param name="id">ID người dùng</param>
        /// <returns>View form chỉnh sửa</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var query = new Application.Queries.Users.GetUserByIdQuery { UserId = id };
                var result = await _userService.GetUserByIdAsync(query);

                if (!result.Success || result.User == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy người dùng";
                    return RedirectToAction(nameof(Index));
                }

                var model = new UserEditViewModel
                {
                    UserId = result.User.UserId,
                    UserName = result.User.UserName,
                    Email = result.User.Email,
                    FirstName = result.User.FirstName,
                    LastName = result.User.LastName,
                    DateOfBirth = result.User.DateOfBirth,
                    Bio = result.User.Bio,
                    IsActive = result.User.IsActive,
                    CreatedAt = result.User.CreatedAt
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user edit form for ID: {UserId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải form chỉnh sửa";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Xử lý cập nhật người dùng
        /// </summary>
        /// <param name="model">Dữ liệu người dùng</param>
        /// <returns>Redirect về danh sách hoặc hiển thị lỗi</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var command = new Application.Commands.Users.UpdateUserCommand
                {
                    UserId = model.UserId,
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Bio = model.Bio,
                    IsActive = model.IsActive
                };

                var result = await _userService.UpdateUserAsync(command);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Cập nhật người dùng thành công";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message ?? "Có lỗi xảy ra khi cập nhật người dùng";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật người dùng";
                return View(model);
            }
        }

        /// <summary>
        /// Hiển thị trang xác nhận xóa người dùng
        /// </summary>
        /// <param name="id">ID người dùng</param>
        /// <returns>View xác nhận xóa</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var query = new Application.Queries.Users.GetUserByIdQuery { UserId = id };
                var result = await _userService.GetUserByIdAsync(query);

                if (!result.Success || result.User == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy người dùng";
                    return RedirectToAction(nameof(Index));
                }

                return View(result.User);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user delete confirmation for ID: {UserId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải thông tin xóa";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Xử lý xóa người dùng
        /// </summary>
        /// <param name="id">ID người dùng</param>
        /// <returns>Redirect về danh sách</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var command = new Application.Commands.Users.DeleteUserCommand { UserId = id };
                var result = await _userService.DeleteUserAsync(command);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Xóa người dùng thành công";
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message ?? "Có lỗi xảy ra khi xóa người dùng";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID: {UserId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa người dùng";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
