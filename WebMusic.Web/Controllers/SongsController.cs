using Microsoft.AspNetCore.Mvc;
using WebMusic.Web.Services;
using WebMusic.Web.ViewModels.Songs;

namespace WebMusic.Web.Controllers
{
    /// <summary>
    /// Controller xử lý các action liên quan đến Songs
    /// </summary>
    public class SongsController : Controller
    {
        private readonly ISongWebService _songWebService;
        private readonly ILogger<SongsController> _logger;

        public SongsController(ISongWebService songWebService, ILogger<SongsController> logger)
        {
            _songWebService = songWebService;
            _logger = logger;
        }

        /// <summary>
        /// Hiển thị danh sách bài hát
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm</param>
        /// <param name="genreId">ID thể loại</param>
        /// <param name="albumId">ID album</param>
        /// <param name="artistId">ID nghệ sĩ</param>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng mỗi trang</param>
        /// <param name="sortBy">Sắp xếp theo</param>
        /// <param name="sortDirection">Hướng sắp xếp</param>
        /// <returns>View danh sách bài hát</returns>
        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchTerm = null,
            int? genreId = null,
            int? albumId = null,
            int? artistId = null,
            int page = 1,
            int pageSize = 10,
            string? sortBy = null,
            string? sortDirection = "desc")
        {
            try
            {
                var model = new SongListViewModel
                {
                    SearchTerm = searchTerm,
                    GenreId = genreId,
                    AlbumId = albumId,
                    ArtistId = artistId,
                    Page = page,
                    PageSize = pageSize,
                    SortBy = sortBy,
                    SortDirection = sortDirection
                };

                var result = await _songWebService.GetSongsAsync(model);
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading songs list");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải danh sách bài hát";
                return View(new SongListViewModel());
            }
        }

        /// <summary>
        /// Hiển thị chi tiết bài hát
        /// </summary>
        /// <param name="id">ID bài hát</param>
        /// <returns>View chi tiết bài hát</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var song = await _songWebService.GetSongByIdAsync(id);
                if (song == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy bài hát";
                    return RedirectToAction(nameof(Index));
                }

                return View(song);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading song details for ID: {SongId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải chi tiết bài hát";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Hiển thị form tạo bài hát mới
        /// </summary>
        /// <returns>View form tạo bài hát</returns>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = await _songWebService.GetCreateViewModelAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create song form");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải form tạo bài hát";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Xử lý tạo bài hát mới
        /// </summary>
        /// <param name="model">Dữ liệu bài hát</param>
        /// <returns>Redirect về danh sách hoặc hiển thị lỗi</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(50 * 1024 * 1024)] // 50MB limit
        public async Task<IActionResult> Create(SongCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Reload dropdown data
                    var createModel = await _songWebService.GetCreateViewModelAsync();
                    model.Genres = createModel.Genres;
                    model.Albums = createModel.Albums;
                    model.Artists = createModel.Artists;
                    return View(model);
                }

                var success = await _songWebService.CreateSongAsync(model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Tạo bài hát thành công";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo bài hát";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating song");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo bài hát";
                return View(model);
            }
        }

        /// <summary>
        /// Hiển thị form chỉnh sửa bài hát
        /// </summary>
        /// <param name="id">ID bài hát</param>
        /// <returns>View form chỉnh sửa</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await _songWebService.GetEditViewModelAsync(id);
                return View(model);
            }
            catch (ArgumentException)
            {
                TempData["ErrorMessage"] = "Không tìm thấy bài hát";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit song form for ID: {SongId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải form chỉnh sửa";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Xử lý cập nhật bài hát
        /// </summary>
        /// <param name="id">ID bài hát</param>
        /// <param name="model">Dữ liệu cập nhật</param>
        /// <returns>Redirect về danh sách hoặc hiển thị lỗi</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SongEditViewModel model)
        {
            try
            {
                if (id != model.SongId)
                {
                    TempData["ErrorMessage"] = "ID không khớp";
                    return RedirectToAction(nameof(Index));
                }

                if (!ModelState.IsValid)
                {
                    // Reload dropdown data
                    var editModel = await _songWebService.GetEditViewModelAsync(id);
                    model.Genres = editModel.Genres;
                    model.Albums = editModel.Albums;
                    model.Artists = editModel.Artists;
                    return View(model);
                }

                var success = await _songWebService.UpdateSongAsync(model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Cập nhật bài hát thành công";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật bài hát";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating song with ID: {SongId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật bài hát";
                return View(model);
            }
        }

        /// <summary>
        /// Hiển thị xác nhận xóa bài hát
        /// </summary>
        /// <param name="id">ID bài hát</param>
        /// <returns>View xác nhận xóa</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var song = await _songWebService.GetSongByIdAsync(id);
                if (song == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy bài hát";
                    return RedirectToAction(nameof(Index));
                }

                return View(song);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading delete confirmation for song ID: {SongId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải thông tin xóa";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Xử lý xóa bài hát
        /// </summary>
        /// <param name="id">ID bài hát</param>
        /// <returns>Redirect về danh sách</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _songWebService.DeleteSongAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Xóa bài hát thành công";
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa bài hát";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting song with ID: {SongId}", id);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa bài hát";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
