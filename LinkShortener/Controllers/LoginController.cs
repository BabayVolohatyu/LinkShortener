using LinkShortener.Data;
using LinkShortener.DTO;
using LinkShortener.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _repository;

        public LoginController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _repository.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "User"
            };

            await _repository.CreateAsync(user);

            return Ok(user);
        }
    }
}
