using Application.Common.BaseModel;
using Application.Contrast.QueryServices;
using Application.DTO.NewsCategory;
using Application.FreamWork.SearchBaseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.News.Ef.Persistance.Query
{
    public class CategoryNewsQueryService : ICategoryNewsQueryService
    {
        private readonly NewsContext db;
        public CategoryNewsQueryService(NewsContext db)
        {
            this.db= db;
            
        }

        public async Task<GenericOperationResult<AllCategoriesListModel>> getAll(PageModel model)
        {
            AllCategoriesListModel result = new AllCategoriesListModel();
            var catList = from iteam in db.newsCategories select iteam;
            var q1 = catList.AsNoTracking().Select(x => new CategoryUpdateModel
            {
                CategoryId = x.CategoryId,
                Description = x.Description,
                Name = x.Name,
                Slug = x.Slug,

            });
            var q2 = await q1.Skip((model.pageIndex - 1) * model.pageSize).Take(model.pageSize).ToListAsync();
            result.RecordCount = await catList.CountAsync();
            result.pageIndex = model.pageIndex;
            result.pageSize = model.pageSize;

            result.categories = q2;
            return GenericOperationResult<AllCategoriesListModel>.ToSuccess("Cayegory lsit", result);
        }

        public async Task<GenericComplexResult<CategorySearchModel, CategoryListIteam>> SearchCategory(CategorySearchModel sm)
        {
            var q1 = from items in db.newsCategories select items;
            q1 = q1.AsNoTracking();
            var result  = new  GenericComplexResult < CategorySearchModel, CategoryListIteam> ();
            if (sm.CategoryId!=null)
            {
                q1 = q1.Where(x => x.CategoryId == sm.CategoryId);

                
            }
            if (sm.Phrase != null)
            {
                q1 = q1.Where(x => x.Name == sm.Phrase || x.Description == sm.Phrase);


            }
            var q2 = await q1.Skip((sm.pageIndex-1)*sm.pageSize ).Take(sm.pageSize).Select(x=> new CategoryListIteam
            {
                CategoryId = x.CategoryId,
                Name = x.Name,
                Slug=x.Slug,
            }).ToListAsync();
            sm.RecordCount = await q1.CountAsync();
            result.ListIteams = q2;
            result.SearchModel = sm;
            return result;




        }
    }
}
