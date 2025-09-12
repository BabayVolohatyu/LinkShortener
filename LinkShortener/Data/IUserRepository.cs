using LinkShortener.Models;

namespace LinkShortener.Data
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<User?> GetByEmailAsync(string email);
    }
}
