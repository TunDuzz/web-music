using Microsoft.AspNetCore.Mvc;
using WebMusic.Application.Commands.Songs;
using WebMusic.Application.Queries.Songs;
using WebMusic.Application.Services;

namespace WebMusic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly ISongService _songService;
        private readonly ILogger<SongsController> _logger;

        public SongsController(ISongService songService, ILogger<SongsController> logger)
        {
            _songService = songService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetSongs([FromQuery] GetSongsQuery query)
        {
            var result = await _songService.GetSongsAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSong(int id)
        {
            var query = new GetSongByIdQuery { SongId = id };
            var result = await _songService.GetSongByIdAsync(query);
            
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSong([FromBody] CreateSongCommand command)
        {
            var result = await _songService.CreateSongAsync(command);
            
            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetSong), new { id = result.Song?.SongId }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromBody] UpdateSongCommand command)
        {
            command.SongId = id;
            var result = await _songService.UpdateSongAsync(command);
            
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var command = new DeleteSongCommand { SongId = id };
            var result = await _songService.DeleteSongAsync(command);
            
            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }
    }
}
