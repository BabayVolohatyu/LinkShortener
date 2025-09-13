using LinkShortener.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly LinkShortenerDbContext _context;

        public UserRepository(LinkShortenerDbContext context) 
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User user) 
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetByIdAsync(int id) 
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email) 
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
