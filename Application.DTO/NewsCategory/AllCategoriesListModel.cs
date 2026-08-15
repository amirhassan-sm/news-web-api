using Application.FreamWork.SearchBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.NewsCategory
{
    public class AllCategoriesListModel:PageModel
    {
        public List<CategoryUpdateModel> categories { get; set; } = new();
    }
}
