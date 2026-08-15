using Domain.News.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.News.Ef.Persistance.Configuration
{
    public class NewsCategoryConfiguration : IEntityTypeConfiguration<NewsCategory>
    {
        public void Configure(EntityTypeBuilder<NewsCategory> builder)
        {
            builder.HasKey(x => x.CategoryId);
            builder.Property(x => x.Slug).HasMaxLength(400).IsRequired(false);
            builder.Property(x => x.Description).HasMaxLength(2000).IsRequired(false);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true);


            builder.HasMany(x=>x.News).WithOne(x=>x.NewsCategory).HasForeignKey(x=>x.CategoryId).OnDelete(DeleteBehavior.Restrict);


        }
    }
}
