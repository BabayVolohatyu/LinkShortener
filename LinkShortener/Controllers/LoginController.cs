using LinkShortener.Data;
using LinkShortener.DTO;
using LinkShortener.Models;
using LinkShortener.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


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

        [HttpGet("me")]
        [Authorize] 
        public IActionResult Me()
        {
            // Ensure we have an authenticated user
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
                return Unauthorized(new { message = "User is not authenticated." });

            // Extract claims
            var idClaim = User.FindFirst("id");
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            var nameClaim = User.FindFirst(ClaimTypes.Name);

            if (idClaim == null || roleClaim == null || nameClaim == null)
                return Unauthorized(new { message = "Invalid or incomplete token." });

            var userDto = new UserDTO
            {
                Id = int.Parse(idClaim.Value),
                Name = nameClaim.Value,
                Email = User.FindFirst("email")?.Value ?? string.Empty, // optional, may be empty
                Role = roleClaim.Value
            };

            return Ok(userDto);
        }
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Append("jwt", "", new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
            });

            return Ok(new { message = "Logged out successfully" });
        }
    }
}
