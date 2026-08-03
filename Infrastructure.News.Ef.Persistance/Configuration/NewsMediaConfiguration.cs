using Domain.News.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.News.Ef.Persistance.Configuration
{
    public class NewsMediaConfiguration : IEntityTypeConfiguration<NewsMedia>
    {
        public void Configure(EntityTypeBuilder<NewsMedia> builder)
        {
            // Primary Key
            builder.HasKey(x => x.NewsMediaId);

            // Properties
            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.AltText)
                .HasMaxLength(200);

            builder.Property(x => x.MediaTypeId)
                .IsRequired();

            builder.Property(x => x.DisplayOrder)
                .HasDefaultValue(1);

            builder.Property(x => x.IsThumbnail)
                .HasDefaultValue(false);

            // Relationship
            builder.HasOne(x => x.News)
                .WithMany(x => x.NewsMedia)
                .HasForeignKey(x => x.NewsId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.NewsId);

            builder.HasIndex(x => new { x.NewsId, x.DisplayOrder });

            builder.HasIndex(x => new { x.NewsId, x.IsThumbnail });
        }
    }
}   