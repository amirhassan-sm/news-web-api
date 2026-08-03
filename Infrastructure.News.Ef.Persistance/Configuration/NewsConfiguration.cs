using Domain.News.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.News.Ef.Persistance.Configuration
{
    public class NewsConfiguration : IEntityTypeConfiguration<Domain.News.Models.News>
    {
        public void Configure(EntityTypeBuilder<Domain.News.Models.News> builder)
        {
            // Primary Key
            builder.HasKey(x => x.NewsId);

            // Properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.Slug)
                .IsRequired(false)
                .HasMaxLength(250);

            builder.HasIndex(x => x.Slug)
                .IsUnique();

            builder.Property(x => x.Summery)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Content)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired().HasDefaultValue(false);

            builder.Property(x => x.ViewCount)
                .HasDefaultValue(0);

            builder.Property(x => x.Metatag)
                .HasMaxLength(400).IsRequired(false);

            builder.Property(x => x.MedtaData)
                .HasMaxLength(400).IsRequired(false);

            builder.Property(x => x.Metaescription)
                .HasMaxLength(400).IsRequired(false);

            builder.Property(x => x.PublishedDate)
                .IsRequired(false);

            builder.Property(x => x.CraetedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired(false);

            // Relationships

            builder.HasOne(x => x.NewsCategory)
                .WithMany(x => x.News)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Comments)
                .WithOne(x => x.News)
                .HasForeignKey(x => x.NewsId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.NewsMedia)
                .WithOne(x => x.News)
                .HasForeignKey(x => x.NewsId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.NewsTags)
                .WithOne(x => x.News)
                .HasForeignKey(x => x.NewsId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}