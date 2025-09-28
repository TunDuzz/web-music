using Microsoft.AspNetCore.Mvc;
using WebMusic.Application.Commands.Albums;
using WebMusic.Application.Commands.Playlists;
using WebMusic.Application.Commands.Songs;
using WebMusic.Application.Services;
using WebMusic.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using WebMusic.Domain.Entities;

namespace WebMusic.Web.Controllers
{
    /// <summary>
    /// Controller để tạo dữ liệu mẫu cho testing
    /// </summary>
    public class SeedDataController : Controller
    {
        private readonly ISongService _songService;
        private readonly IAlbumService _albumService;
        private readonly IPlaylistService _playlistService;
        private readonly IGenreRepository _genreRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<SeedDataController> _logger;

        public SeedDataController(
            ISongService songService,
            IAlbumService albumService,
            IPlaylistService playlistService,
            IGenreRepository genreRepository,
            IArtistRepository artistRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<SeedDataController> logger)
        {
            _songService = songService;
            _albumService = albumService;
            _playlistService = playlistService;
            _genreRepository = genreRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _artistRepository = artistRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSampleData()
        {
            try
            {
                // Tạo Roles trước
                await CreateRolesAsync();

                // Tạo User admin
                var adminUser = await CreateAdminUserAsync();

                // Tạo Genre
                var genre = new WebMusic.Domain.Entities.Genre
                {
                    GenreName = "Pop",
                    Description = "Pop music genre",
                    CreatedAt = DateTime.UtcNow
                };
                await _genreRepository.AddGenreAsync(genre);

                // Tạo Artist
                var artist = new WebMusic.Domain.Entities.Artist
                {
                    ArtistName = "Test Artist",
                    Biography = "Test artist biography",
                    CreatedAt = DateTime.UtcNow
                };
                await _artistRepository.AddArtistAsync(artist);

                // Tạo Album mẫu
                var albumCommand = new CreateAlbumCommand
                {
                    AlbumName = "Album Demo",
                    Description = "Đây là album demo để test",
                    ReleaseDate = DateTime.Now.AddDays(-30),
                    CoverImage = "https://via.placeholder.com/300x300/007bff/ffffff?text=Demo+Album",
                    UserId = adminUser.Id
                };
                var albumResult = await _albumService.CreateAlbumAsync(albumCommand);

                // Tạo Song mẫu
                var songCommand = new CreateSongCommand
                {
                    Title = "Bài hát demo",
                    FileUrl = "https://www.soundjay.com/misc/sounds/bell-ringing-05.wav",
                    CoverImage = "https://via.placeholder.com/300x300/28a745/ffffff?text=Demo+Song",
                    Duration = 180, // 3 phút
                    UserId = adminUser.Id,
                    GenreId = genre.GenreId,
                    AlbumId = albumResult.Success ? albumResult.Album?.AlbumId : null,
                    ArtistId = artist.ArtistId
                };
                var songResult = await _songService.CreateSongAsync(songCommand);

                // Tạo Playlist mẫu
                var playlistCommand = new CreatePlaylistCommand
                {
                    PlaylistName = "Playlist Demo",
                    Description = "Đây là playlist demo để test",
                    IsPublic = true,
                    UserId = adminUser.Id
                };
                var playlistResult = await _playlistService.CreatePlaylistAsync(playlistCommand);

                TempData["SuccessMessage"] = "Đã tạo dữ liệu mẫu thành công! Bao gồm: Roles, Admin User, Genre, Artist, Album, Song, Playlist";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating sample data");
                TempData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task CreateRolesAsync()
        {
            var roles = new[] { "Admin", "Moderator", "User" };

            foreach (var roleName in roles)
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

        private async Task<ApplicationUser> CreateAdminUserAsync()
        {
            var adminEmail = "admin@example.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    IsActive = true,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            return adminUser;
        }
    }
}