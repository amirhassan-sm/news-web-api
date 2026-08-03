using Microsoft.EntityFrameworkCore;
using News.DomainServiceContract.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.News.Ef.Persistance.Repository
{
    public class NewsRepository : INewsRepository
    {
        private readonly NewsContext db;
        public NewsRepository(NewsContext db)
        {
            this.db = db;
            
        }
        public async Task AddAsync(Domain.News.Models.News model)
        {
            model.CraetedAt = DateTime.UtcNow;
            db.News.Add(model);
            await db.SaveChangesAsync();
           
        }

        public async Task AddNewsViewCountAsync(int id, int number)
        {
            var news = await db.News.FirstOrDefaultAsync(x=>x.NewsId==id);
            if (news !=null)
            {
                news.ViewCount = news.ViewCount + number;
                await db.SaveChangesAsync();
                
            }
            else
            {
                return;
            }
        }

        public async Task<Domain.News.Models.News?> GetAsync(int id)
        {
            return await db.News.FindAsync(id);
        }

        public async Task<Domain.News.Models.News?> GetBySlugAsync(string slug)
        {
            return await db.News.FirstOrDefaultAsync(x => x.Slug == slug);
        }

        public async Task<List<Domain.News.Models.News>> GetLatestAsync(int count)
        {
           return await db.News.OrderByDescending(x=>x.NewsId).Take(count).ToListAsync();
        }

        public async Task<bool> IsCategoryExist(int categoryId)
        {
            return await db.newsCategories.AnyAsync(x=>x.CategoryId==categoryId);
        }

        public async Task<bool> IsNewsExistsAsync(int id)
        {
            return await db.News.AnyAsync(x => x.NewsId == id);
        }

        public async Task<bool> IsNewsSlugExistAsync(string slug)
        {
            return await db.News.AnyAsync(x => x.Slug == slug);
        }

        public async Task<bool> IsNewsSlugExistsExceptCurrentAsync(string slug, int id)
        {
            return await db.News.AnyAsync(x=>x.NewsId != id&& x.Slug == slug );
        }

        public async Task<bool> IsNewsTitleExistAsync(string name)
        {
            return await db.News.AnyAsync(x => x.Title == name);
        }

        public async Task<bool> IsNewsTitleExistsExceptCurrentAsync(string name, int id)
        {
            return await db.News.AnyAsync(x=>x.NewsId != id&& x.Title == name);
        }

        public async Task PublishNewsAsync(int newsId)
        {
           var news = await db.News.FirstOrDefaultAsync(x=>x.NewsId ==newsId);
            if (news!=null)
            {
                news?.Status = true;
                news?.PublishedDate = DateTime.UtcNow;
                await db.SaveChangesAsync();

            }
            else
            {
                await Task.CompletedTask;
            }
            
        }

        public async Task RemoveAsync(int id)
        {
            var news = await db.News.FirstOrDefaultAsync(x=>x.NewsId==id);
            if (news == null)
            {
                await Task.CompletedTask;

            }
            else { 
                db.News.Remove(news);
                await db.SaveChangesAsync();
            
            
            }
        }

        public async Task UpdateAsync(Domain.News.Models.News model)
        {
            var news = await db.News.FindAsync(model.NewsId);
            if (news != null)
            {
               news.Title = model.Title;
                news.Status = model.Status;
                news.Summery = model.Summery;
                news.AuthorId = model.AuthorId;
                news.CategoryId = model.CategoryId;
                news.Content = model.Content;
                news.UpdatedAt = DateTime.UtcNow;
                news.Metatag = model.Metatag;
                news.Metaescription = model.Metaescription;
                news.MedtaData = model.MedtaData;
                await db.SaveChangesAsync();




            }
            else
            {
                await Task.CompletedTask;
            }
        }
    }
}
