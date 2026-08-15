using Application.FreamWork.SearchBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.NewsCategory
{
    public class CategorySearchModel:PageModel
    {
        public int? CategoryId { get; set; }
        public string? Phrase { get; set; } 
    }
}
