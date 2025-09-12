using LinkShortener.Models;

namespace LinkShortener.Data
{
    public interface IUserRepository
    {
        Task<User> Create(User user);
        Task<User?> GetByEmailAsync(string email);
    }
}
