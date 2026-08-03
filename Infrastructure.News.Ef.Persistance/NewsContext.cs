using Domain.News.Models;
using Infrastructure.News.Ef.Persistance.Configuration;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;


namespace Infrastructure.News.Ef.Persistance
{
    public class NewsContext:DbContext
    {
        public NewsContext(DbContextOptions<NewsContext> options):base(options)
        {
            
        }
        public DbSet<Domain.News.Models.News> News { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<NewsCategory> newsCategories { get; set; }
        public DbSet<NewsMedia> NewsMedias { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new NewsCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new NewsMediaConfiguration());
            modelBuilder.ApplyConfiguration(new NewsTagConfiguration());
            modelBuilder.ApplyConfiguration(new TagsConfiguration());

            base.OnModelCreating(modelBuilder);
        }




    }
}
