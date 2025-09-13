using LinkShortener.Data;
using LinkShortener.DTO;
using LinkShortener.Models;
using LinkShortener.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Controllers
{
    [Route("")]
    public class LoginController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IJwtService _jwtService;

        public LoginController(IUserRepository repository, IJwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _repository.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var jwt = _jwtService.Generate(user.Id, user.Role, user.Name);

            Response.Cookies.Append("jwt", jwt, new CookieOptions 
            {
                HttpOnly = true
            });

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(new
            {
                token = jwt,
                user = userDTO
            });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {

            var existingUser = await _repository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Email already in use" });
            }
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "User"
            };

            await _repository.CreateAsync(user);

            var jwt = _jwtService.Generate(user.Id, user.Role, user.Name);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(new
            {
                token = jwt,
                user = userDTO
            });

        }
    }
}
