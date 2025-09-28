using Microsoft.AspNetCore.Mvc;
using WebMusic.Application.Services;
using WebMusic.Web.Services;

namespace WebMusic.Web.Controllers
{
    /// <summary>
    /// Controller xử lý trang chủ và dashboard
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ISongService _songService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ISongService songService,
            ILogger<HomeController> logger)
        {
            _songService = songService;
            _logger = logger;
        }

        /// <summary>
        /// Hiển thị trang chủ với dashboard
        /// </summary>
        /// <returns>View trang chủ</returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                // Lấy thống kê tổng quan
                var stats = await GetDashboardStatsAsync();
                
                // Lấy bài hát mới nhất
                var recentSongsQuery = new WebMusic.Application.Queries.Songs.GetSongsQuery
                {
                    Page = 1,
                    PageSize = 6,
                    SortBy = "UploadedAt",
                    SortDirection = "desc"
                };
                var recentSongs = await _songService.GetSongsAsync(recentSongsQuery);

                // Lấy bài hát phổ biến
                var popularSongsQuery = new WebMusic.Application.Queries.Songs.GetSongsQuery
                {
                    Page = 1,
                    PageSize = 6,
                    SortBy = "ViewCount",
                    SortDirection = "desc"
                };
                var popularSongs = await _songService.GetSongsAsync(popularSongsQuery);

                ViewBag.Stats = stats;
                ViewBag.RecentSongs = recentSongs.Songs;
                ViewBag.PopularSongs = popularSongs.Songs;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải trang chủ";
                return View();
            }
        }

        /// <summary>
        /// Hiển thị trang Privacy
        /// </summary>
        /// <returns>View Privacy</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Hiển thị trang About
        /// </summary>
        /// <returns>View About</returns>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Hiển thị trang Contact
        /// </summary>
        /// <returns>View Contact</returns>
        public IActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// Lấy thống kê dashboard
        /// </summary>
        /// <returns>Object chứa thống kê</returns>
        private async Task<object> GetDashboardStatsAsync()
        {
            try
            {
                // Lấy tổng số bài hát
                var totalSongsQuery = new WebMusic.Application.Queries.Songs.GetSongsQuery
                {
                    Page = 1,
                    PageSize = 1
                };
                var totalSongs = await _songService.GetSongsAsync(totalSongsQuery);

                // TODO: Implement other services when available
                // var totalUsers = await _userService.GetTotalCountAsync();
                // var totalAlbums = await _albumService.GetTotalCountAsync();
                // var totalPlaylists = await _playlistService.GetTotalCountAsync();

                return new
                {
                    TotalSongs = totalSongs.TotalCount,
                    TotalUsers = 0, // Placeholder
                    TotalAlbums = 0, // Placeholder
                    TotalPlaylists = 0 // Placeholder
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard stats");
                return new
                {
                    TotalSongs = 0,
                    TotalUsers = 0,
                    TotalAlbums = 0,
                    TotalPlaylists = 0
                };
            }
        }
    }
}