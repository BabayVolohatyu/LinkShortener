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

            var jwt = _jwtService.Generate(user.Id, user.Role);

            Response.Cookies.Append("jwt", jwt, new CookieOptions 
            {
                HttpOnly = true
            });

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
