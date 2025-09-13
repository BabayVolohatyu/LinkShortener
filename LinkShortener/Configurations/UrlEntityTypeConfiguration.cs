using LinkShortener.Models;
using LinkShortener.Services;
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

            builder 
                .HasIndex(u => u.Code)
                .IsUnique();
            builder
                .Property(u => u.Code)
                .HasMaxLength(UrlShorteningService._numberOfChars);

            builder
                .HasOne(u => u.User)
                .WithMany(user => user.Urls)
                .HasForeignKey(u => u.CreatedById);
        }
    }
}
