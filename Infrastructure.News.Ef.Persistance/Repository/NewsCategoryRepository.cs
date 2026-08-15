using Domain.News.Models;
using Microsoft.EntityFrameworkCore;
using News.DomainServiceContract.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.News.Ef.Persistance.Repository
{
    public class NewsCategoryRepository : INewsCategoryRepository
    {
        private readonly NewsContext db;
        public NewsCategoryRepository(NewsContext db)
        {
            this.db = db;
            
        }
        public async Task AddAsync(NewsCategory model)
        {
            db.newsCategories.Add(model);
            await db.SaveChangesAsync();

        }

        public async Task<List<NewsCategory>> GetAllNewsCategories()
        {
            return await db.newsCategories.ToListAsync();
        }

        public async Task<NewsCategory?> GetAsync(int id)
        {
            return await db.newsCategories.FindAsync(id);
        }

        public async Task<bool> IsCategoryNameExistAsync(string name)
        {
            return await db.newsCategories.AnyAsync(x => x.Name == name);
        }

        public async Task<bool> IsCategoryNameExistExceptCurrentAsync(int categoryId, string categoryName)
        {
            return await db.newsCategories.AnyAsync(x => x.Name == categoryName && x.CategoryId != categoryId);

        }

        public async Task<bool> IsCategorySlugExistAsync(string slug)
        {
            return await db.newsCategories.AnyAsync(x => x.Slug == slug);
        }

        public async Task<bool> IsCategorySlugExistExceptCurrentAsync(int categoryId, string categorySlug)
        {
            return await db.newsCategories.AnyAsync(x => x.Slug == categorySlug && x.CategoryId != categoryId);
        }

        public async Task<bool> NewsCategoryHasChild(int categoryId)
        {
           return await db.News.AnyAsync(x=>x.CategoryId == categoryId);
        }

        public async Task RemoveAsync(int id)
        {
           var cat = await db.newsCategories.FirstOrDefaultAsync(x=>x.CategoryId== id);
            if (cat == null)
            {
                return  ; 
                
            }
            db.newsCategories.Remove(cat);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(NewsCategory model)
        {
            var category = await db.newsCategories.FirstOrDefaultAsync(x => x.CategoryId == model.CategoryId);
            if (category == null)
            {
                return;
                
            }
            category.Name = model.Name;
            category.Slug = model.Slug;
            category.Description = model.Description;
            await db.SaveChangesAsync();

        }
    }
}
