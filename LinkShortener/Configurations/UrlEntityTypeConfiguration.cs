using LinkShortener.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkShortener.Configurations
{
    public class UrlEntityTypeConfiguration : IEntityTypeConfiguration<Url>
    {
        public void Configure(EntityTypeBuilder<Url> builder) 
        {
            builder
                .HasKey(u => u.Id);

            builder
                .HasIndex(u => u.OriginalUrl)
                .IsUnique();

            builder
                .HasIndex(u => u.ShortUrl)
                .IsUnique();
        }
    }
}
