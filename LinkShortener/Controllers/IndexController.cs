using LinkShortener.Data;
using LinkShortener.DTO;
using LinkShortener.Models;
using LinkShortener.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Controllers
{
    [Route("index")]
    public class IndexController : Controller
    {
        private readonly IUrlRepository _urlRepository;

        public IndexController(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var urls = await _urlRepository.GetAllAsync();
            var dto = urls.Select(u => new UrlDTO
            {
                Id = u.Id,
                ShortUrl = u.ShortUrl,
                OriginalUrl = u.OriginalUrl,
                CreatedBy = u.CreatedBy,
                CreatedDate = u.CreatedDate,
                Description = u.Description
            });
            return Ok(dto);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var url = await _urlRepository.GetByIdAsync(id);
            if (url == null) return NotFound();

            var dto = new UrlDTO
            {
                Id = url.Id,
                ShortUrl = url.ShortUrl,
                OriginalUrl = url.OriginalUrl,
                CreatedBy = url.CreatedBy,
                CreatedDate = url.CreatedDate,
                Description = url.Description
            };
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create(
            [FromBody] UrlDTO dto,
            IUrlShorteningService urlShorteningService,
            HttpContext httpContext)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if URL already exists
            var existing = await _urlRepository.GetByOriginalUrlAsync(dto.OriginalUrl);
            if (existing != null)
                return Conflict(new { message = "This URL already exists." });

            // Get current user ID from token
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null) return Unauthorized();
            var currentUserId = int.Parse(userIdClaim.Value);

            var userName = User.Identity?.Name;

            // Generate short URL
            if(!Uri.TryCreate(dto.OriginalUrl, UriKind.Absolute, out _))
            {
                return BadRequest("The URL is invalid");
            }

            var code = await urlShorteningService.GenerateUniqueCode();

            var shortenedUrl = $"{httpContext.Request.Scheme}:://{httpContext.Request.Host}/{code}";

            var url = new Url
            {
                OriginalUrl = dto.OriginalUrl,
                Code = code,
                ShortUrl = shortenedUrl,
                Description = dto.Description,
                CreatedBy = userName?? "Unknown",
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _urlRepository.CreateAsync(url);

            var result = new UrlDTO
            {
                Id = url.Id,
                ShortUrl = url.ShortUrl,
                OriginalUrl = url.OriginalUrl,
                CreatedBy = url.CreatedBy,
                CreatedDate = url.CreatedDate,
                Description = url.Description
            };

            return Ok(result);
        }


        [HttpPost("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(int id)
        {
            var url = await _urlRepository.GetByIdAsync(id);
            if (url == null) return NotFound();

            var currentUserName = User.Identity?.Name;

            if (User.IsInRole("Admin") || url.CreatedBy == currentUserName)
            {
                await _urlRepository.DeleteAsync(id);
                return NoContent();
            }

            return Forbid();
        }


    }
}
