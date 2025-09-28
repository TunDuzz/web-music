using Microsoft.AspNetCore.Mvc;
using WebMusic.Application.DTOs;
using WebMusic.Application.Services;
using WebMusic.Application.Commands.Users;
using WebMusic.Application.Queries.Users;

namespace WebMusic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] string? searchTerm = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetUsersQuery
            {
                SearchTerm = searchTerm,
                Page = page,
                PageSize = pageSize
            };
            
            var result = await _userService.GetUsersAsync(query);
            
            if (!result.Success)
                return BadRequest(result.Message);
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var query = new GetUserByIdQuery { UserId = id };
            var result = await _userService.GetUserByIdAsync(query);
            
            if (!result.Success || result.User == null)
                return NotFound(result.Message);

            return Ok(result.User);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            try
            {
                var result = await _userService.CreateUserAsync(command);
                
                if (!result.Success)
                    return BadRequest(result.Message);
                
                return CreatedAtAction(nameof(GetUser), new { id = result.User?.UserId }, result.User);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserCommand command)
        {
            try
            {
                command.UserId = id;
                var result = await _userService.UpdateUserAsync(command);
                
                if (!result.Success)
                    return BadRequest(result.Message);
                
                return Ok(result.User);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var command = new DeleteUserCommand { UserId = id };
            var result = await _userService.DeleteUserAsync(command);
            
            if (!result.Success)
                return NotFound(result.Message);

            return NoContent();
        }

        [HttpPost("check-email")]
        public async Task<IActionResult> CheckEmailExists([FromBody] string email)
        {
            var exists = await _userService.EmailExistsAsync(email);
            return Ok(new { exists });
        }

        [HttpPost("check-username")]
        public async Task<IActionResult> CheckUsernameExists([FromBody] string username)
        {
            var exists = await _userService.UsernameExistsAsync(username);
            return Ok(new { exists });
        }
    }
}
