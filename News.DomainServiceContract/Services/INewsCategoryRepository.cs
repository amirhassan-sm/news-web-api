using Domain.News.Models;
using News.Freamwork.BaseInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.DomainServiceContract.Services
{
    public interface INewsCategoryRepository:BaseInterface<NewsCategory,int>
    {
        Task<List<NewsCategory>> GetAllNewsCategories();
        
        Task<bool> IsCategoryNameExistAsync(string  name);
        Task<bool> IsCategorySlugExistAsync(string slug);

        Task<bool> IsCategoryNameExistExceptCurrentAsync(int categoryId, string categoryName);
        Task<bool> IsCategorySlugExistExceptCurrentAsync(int categoryId, string categorySlug);
        Task<bool> NewsCategoryHasChild(int categoryId);


    }
}
