using LinkShortener.Data;
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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
