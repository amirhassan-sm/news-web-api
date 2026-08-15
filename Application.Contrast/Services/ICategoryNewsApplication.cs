using Application.Common.BaseModel;
using Application.DTO.NewsCategory;
using Application.FreamWork.OperatonResult;
using Application.FreamWork.SearchBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contrast.Services
{
    public interface ICategoryNewsApplication
    {
        Task<OperationResult> AddNewsCategory(CategoryAddModel model );

        Task<OperationResult> RemoveNewsCategory(int id);
        Task<OperationResult> UpdateNewsCategory(CategoryUpdateModel model);
        Task<GenericOperationResult<CategoryUpdateModel>> GetCategoryNewsById(int id);


        Task<GenericOperationResult<AllCategoriesListModel>> GetAllCategories(PageModel page);

        Task<GenericComplexResult<CategorySearchModel, CategoryListIteam>> SearchCategory(CategorySearchModel sm);
    }
}
