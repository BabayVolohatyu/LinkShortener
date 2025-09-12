using LinkShortener.Models;
using System.Runtime.InteropServices;

namespace LinkShortener.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly LinkShortenerDbContext _context;

        public UserRepository(LinkShortenerDbContext context) 
        {
            _context = context;
        }
        public async Task<User> Create(User user) 
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
