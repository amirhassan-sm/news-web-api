using Domain.News.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.News.Ef.Persistance.Configuration
{
    public class NewsTagConfiguration : IEntityTypeConfiguration<NewsTag>
    {
        public void Configure(EntityTypeBuilder<NewsTag> builder)
        {
            builder.HasKey(x => x.NewsTagId);


            builder.HasOne(x => x.News).WithMany(x => x.NewsTags).HasForeignKey(x => x.NewsId);
            builder.HasOne(x => x.Tag).WithMany(x => x.NewsTags).HasForeignKey(x => x.NewsId);

        }
    }
}
