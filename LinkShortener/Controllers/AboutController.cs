using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Controllers
{
    public class AboutController : Controller
    {
        private static string aboutText = "Welcome to LinkShortener. This service lets you create, manage, and share short links easily.";
        private readonly IConfiguration _config;
        public AboutController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("/about")]
        public ActionResult About()
        {
            ViewBag.AboutText = aboutText;
            return View();
        }

        [HttpPost("/about")]
        [Authorize(Roles = "Admin")]
        public ActionResult Save(string updatedText)
        {
            aboutText = updatedText;
            ViewBag.AboutText = aboutText;
            ViewBag.Message = "About text updated successfully!";

            string angularUrl = _config["AllowedOrigins"] ?? "/";

            return Redirect($"{angularUrl}/index");
        }
    }
}
