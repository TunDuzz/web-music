using Microsoft.AspNetCore.Mvc;
using WebMusic.Web.Services;
using WebMusic.Web.ViewModels.Playlists;

namespace WebMusic.Web.Controllers
{
    /// <summary>
    /// Controller xử lý các action liên quan đến Playlists
    /// </summary>
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistWebService _playlistWebService;
        private readonly ILogger<PlaylistsController> _logger;

        public PlaylistsController(IPlaylistWebService playlistWebService, ILogger<PlaylistsController> logger)
        {
            _playlistWebService = playlistWebService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchTerm = null,
            int? userId = null,
            bool? isPublic = null,
            int page = 1,
            int pageSize = 10,
            string? sortBy = null,
            string? sortDirection = "desc")
        {
            try
            {
                var model = new PlaylistListViewModel
                {
                    SearchTerm = searchTerm,
                    UserId = userId,
                    IsPublic = isPublic,
                    Page = page,
                    PageSize = pageSize,
                    SortBy = sortBy,
                    SortDirection = sortDirection
                };

                var result = await _playlistWebService.GetPlaylistsAsync(model);
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading playlists list");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải danh sách playlist";
                return View(new PlaylistListViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var playlist = await _playlistWebService.GetPlaylistByIdAsync(id);
                if (playlist == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy playlist";
                    return RedirectToAction(nameof(Index));
                }

                return View(playlist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading playlist details for ID: {PlaylistId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải chi tiết playlist";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = await _playlistWebService.GetCreateViewModelAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create playlist form");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải form tạo playlist";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlaylistCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var success = await _playlistWebService.CreatePlaylistAsync(model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Tạo playlist thành công";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo playlist";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating playlist");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo playlist";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _playlistWebService.GetEditViewModelAsync(id);
                return View(model);
            }
            catch (ArgumentException)
            {
                TempData["ErrorMessage"] = "Không tìm thấy playlist";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit playlist form for ID: {PlaylistId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải form chỉnh sửa";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlaylistEditViewModel model)
        {
            try
            {
                if (id != model.PlaylistId)
                {
                    TempData["ErrorMessage"] = "ID không khớp";
                    return RedirectToAction(nameof(Index));
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var success = await _playlistWebService.UpdatePlaylistAsync(model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Cập nhật playlist thành công";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật playlist";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating playlist with ID: {PlaylistId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật playlist";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var playlist = await _playlistWebService.GetPlaylistByIdAsync(id);
                if (playlist == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy playlist";
                    return RedirectToAction(nameof(Index));
                }

                return View(playlist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading delete confirmation for playlist ID: {PlaylistId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải thông tin xóa";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _playlistWebService.DeletePlaylistAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Xóa playlist thành công";
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa playlist";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting playlist with ID: {PlaylistId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa playlist";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
