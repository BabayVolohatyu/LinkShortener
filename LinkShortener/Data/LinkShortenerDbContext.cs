using LinkShortener.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Data
{
    public class LinkShortenerDbContext : DbContext
    {
        public LinkShortenerDbContext(DbContextOptions<LinkShortenerDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Url> Urls { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LinkShortenerDbContext).Assembly);



            //for example only
            modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 5555,
                Name = "Test Admin",
                Email = "admin@test.com",
                Password = BCrypt.Net.BCrypt.HashPassword("0000"),   
                Role = "Admin"
            },
            new User
            {
                Id = 6666,
                Name = "Test User",
                Email = "user@test.com",
                Password = BCrypt.Net.BCrypt.HashPassword("0000"),  
                Role = "User"
            }
        );
        }
    }
}
