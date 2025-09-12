using LinkShortener.Data;
using LinkShortener.DTO;
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
            var user = _repository.GetByEmailAsync(request.Email);
            if (user == null) {
               
            }
            return Ok();
        }
    }
}
