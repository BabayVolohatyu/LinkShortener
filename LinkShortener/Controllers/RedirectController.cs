using LinkShortener.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("")]
public class RedirectController : Controller
{
    private readonly IUrlRepository _urlRepository;

    public RedirectController(IUrlRepository urlRepository)
    {
        _urlRepository = urlRepository;
    }

    [HttpGet("{code}")]
    [AllowAnonymous]
    public async Task<IActionResult> RedirectToOriginal(string code)
    {
        var url = await _urlRepository.GetByCodeAsync(code);
        if (url == null)
            return NotFound();

        return Redirect(url.OriginalUrl);
    }
}

