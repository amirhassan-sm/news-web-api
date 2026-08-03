using Domain.News.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.News.Ef.Persistance.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.CommentId);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.IsAproved).IsRequired().HasDefaultValue(false);


            builder.HasOne(x => x.News).WithMany(x => x.Comments).HasForeignKey(x => x.CommentId);
    



        }
    }
}
