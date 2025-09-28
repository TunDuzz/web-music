using Microsoft.AspNetCore.Mvc;
using WebMusic.Application.DTOs;
using WebMusic.Domain.Interfaces;

namespace WebMusic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly ILogger<AlbumsController> _logger;

        public AlbumsController(IAlbumRepository albumRepository, ILogger<AlbumsController> logger)
        {
            _albumRepository = albumRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlbums([FromQuery] int? userId = null, [FromQuery] int? artistId = null, [FromQuery] string? searchTerm = null)
        {
            try
            {
                var albums = userId.HasValue
                    ? await _albumRepository.GetAlbumsByUserIdAsync(userId.Value)
                    : artistId.HasValue
                    ? await _albumRepository.GetAlbumsByArtistIdAsync(artistId.Value)
                    : !string.IsNullOrEmpty(searchTerm)
                    ? await _albumRepository.SearchAlbumsAsync(searchTerm)
                    : await _albumRepository.GetAllAlbumsAsync();

                var albumDtos = albums.Select(MapToAlbumDto);
                return Ok(albumDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving albums");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlbum(int id)
        {
            try
            {
                var album = await _albumRepository.GetAlbumByIdAsync(id);
                if (album == null)
                    return NotFound();

                var albumDto = MapToAlbumDto(album);
                return Ok(albumDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving album {AlbumId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlbum([FromBody] CreateAlbumRequest request)
        {
            try
            {
                var album = new WebMusic.Domain.Entities.Album
                {
                    AlbumName = request.AlbumName,
                    Description = request.Description,
                    ReleaseDate = request.ReleaseDate,
                    CoverImage = request.CoverImage,
                    UserId = request.UserId,
                    ArtistId = request.ArtistId
                };

                var createdAlbum = await _albumRepository.AddAlbumAsync(album);
                var albumDto = MapToAlbumDto(createdAlbum);

                return CreatedAtAction(nameof(GetAlbum), new { id = albumDto.AlbumId }, albumDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating album");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlbum(int id, [FromBody] UpdateAlbumRequest request)
        {
            try
            {
                var existingAlbum = await _albumRepository.GetAlbumByIdAsync(id);
                if (existingAlbum == null)
                    return NotFound();

                existingAlbum.AlbumName = request.AlbumName;
                existingAlbum.Description = request.Description;
                existingAlbum.ReleaseDate = request.ReleaseDate;
                existingAlbum.CoverImage = request.CoverImage;
                existingAlbum.ArtistId = request.ArtistId;
                existingAlbum.UpdatedAt = DateTime.UtcNow;

                var updatedAlbum = await _albumRepository.UpdateAlbumAsync(existingAlbum);
                var albumDto = MapToAlbumDto(updatedAlbum);

                return Ok(albumDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating album {AlbumId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            try
            {
                var album = await _albumRepository.GetAlbumByIdAsync(id);
                if (album == null)
                    return NotFound();

                await _albumRepository.DeleteAlbumAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting album {AlbumId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        private static AlbumDto MapToAlbumDto(WebMusic.Domain.Entities.Album album)
        {
            return new AlbumDto
            {
                AlbumId = album.AlbumId,
                AlbumName = album.AlbumName,
                Description = album.Description,
                ReleaseDate = album.ReleaseDate,
                CoverImage = album.CoverImage,
                CreatedAt = album.CreatedAt,
                UpdatedAt = album.UpdatedAt,
                SongCount = album.SongCount,
                UserId = album.UserId,
                UserName = album.User?.UserName,
                ArtistId = album.ArtistId,
                ArtistName = album.Artist?.ArtistName
            };
        }
    }

    public class CreateAlbumRequest
    {
        public string AlbumName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? CoverImage { get; set; }
        public int UserId { get; set; }
        public int? ArtistId { get; set; }
    }

    public class UpdateAlbumRequest
    {
        public string AlbumName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? CoverImage { get; set; }
        public int? ArtistId { get; set; }
    }
}
