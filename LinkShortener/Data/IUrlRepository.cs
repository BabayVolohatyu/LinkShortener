using LinkShortener.Models;

namespace LinkShortener.Data
{
    public interface IUrlRepository
    {
        Task<IEnumerable<Url>> GetAllAsync();
        Task<Url?> GetByIdAsync(int id);
        Task<Url?> GetByOriginalUrlAsync(string originalUrl);
        Task<Url?> GetByShortUrlAsync(string shortUrl);
        Task<Url?> GetByCodeAsync(string code);
        Task<Url> CreateAsync(Url url);
        Task DeleteAsync(int id);
    }
}
