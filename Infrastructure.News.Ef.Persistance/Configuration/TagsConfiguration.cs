using Domain.News.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.News.Ef.Persistance.Configuration
{
    public class TagsConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(x => x.TagId);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Slug).IsRequired(false).HasMaxLength(250);

            builder.HasMany(x => x.NewsTags).WithOne(x => x.Tag).HasForeignKey(x => x.TagId);
        }
    }
}
