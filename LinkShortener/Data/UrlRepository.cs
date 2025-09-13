using LinkShortener.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Data
{
    public class UrlRepository : IUrlRepository
    {
        private readonly LinkShortenerDbContext _context;

        public UrlRepository(LinkShortenerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Url>> GetAllAsync() 
        {
            return await _context.Urls.ToListAsync();
        }

        public async Task<Url?> GetByIdAsync(int id) 
        {
            return await _context.Urls.FindAsync(id);
        }

        public async Task<Url> CreateAsync(Url url)
        {
            await _context.AddAsync(url);
            await _context.SaveChangesAsync();
            return url;
        }

        public async Task DeleteAsync(int id)
        {
            var url = await _context.Urls.FindAsync(id);
            if (url != null) 
            {
                _context.Urls.Remove(url);
                await _context.SaveChangesAsync();
            }
        }

    }
}
