using Application.Common.BaseModel;
using Application.DTO.NewsCategory;
using Application.FreamWork.SearchBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contrast.QueryServices
{
    public interface ICategoryNewsQueryService
    {
        Task<GenericComplexResult<CategorySearchModel, CategoryListIteam>> SearchCategory(CategorySearchModel sm);
        Task<GenericOperationResult<AllCategoriesListModel>> getAll(PageModel model);
    }
}
