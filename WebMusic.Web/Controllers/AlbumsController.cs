using Microsoft.AspNetCore.Mvc;
using WebMusic.Web.Services;
using WebMusic.Web.ViewModels.Albums;

namespace WebMusic.Web.Controllers
{
    /// <summary>
    /// Controller xử lý các action liên quan đến Albums
    /// </summary>
    public class AlbumsController : Controller
    {
        private readonly IAlbumWebService _albumWebService;
        private readonly ILogger<AlbumsController> _logger;

        public AlbumsController(IAlbumWebService albumWebService, ILogger<AlbumsController> logger)
        {
            _albumWebService = albumWebService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchTerm = null,
            int? artistId = null,
            int page = 1,
            int pageSize = 10,
            string? sortBy = null,
            string? sortDirection = "desc")
        {
            try
            {
                var model = new AlbumListViewModel
                {
                    SearchTerm = searchTerm,
                    ArtistId = artistId,
                    Page = page,
                    PageSize = pageSize,
                    SortBy = sortBy,
                    SortDirection = sortDirection
                };

                var result = await _albumWebService.GetAlbumsAsync(model);
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading albums list");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải danh sách album";
                return View(new AlbumListViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var album = await _albumWebService.GetAlbumByIdAsync(id);
                if (album == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy album";
                    return RedirectToAction(nameof(Index));
                }

                return View(album);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading album details for ID: {AlbumId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải chi tiết album";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = await _albumWebService.GetCreateViewModelAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create album form");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải form tạo album";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlbumCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var createModel = await _albumWebService.GetCreateViewModelAsync();
                    model.Artists = createModel.Artists;
                    return View(model);
                }

                var success = await _albumWebService.CreateAlbumAsync(model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Tạo album thành công";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo album";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating album");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo album";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _albumWebService.GetEditViewModelAsync(id);
                return View(model);
            }
            catch (ArgumentException)
            {
                TempData["ErrorMessage"] = "Không tìm thấy album";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit album form for ID: {AlbumId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải form chỉnh sửa";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AlbumEditViewModel model)
        {
            try
            {
                if (id != model.AlbumId)
                {
                    TempData["ErrorMessage"] = "ID không khớp";
                    return RedirectToAction(nameof(Index));
                }

                if (!ModelState.IsValid)
                {
                    var editModel = await _albumWebService.GetEditViewModelAsync(id);
                    model.Artists = editModel.Artists;
                    return View(model);
                }

                var success = await _albumWebService.UpdateAlbumAsync(model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Cập nhật album thành công";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật album";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating album with ID: {AlbumId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật album";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var album = await _albumWebService.GetAlbumByIdAsync(id);
                if (album == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy album";
                    return RedirectToAction(nameof(Index));
                }

                return View(album);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading delete confirmation for album ID: {AlbumId}", id);
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
                var success = await _albumWebService.DeleteAlbumAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Xóa album thành công";
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa album";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting album with ID: {AlbumId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa album";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
