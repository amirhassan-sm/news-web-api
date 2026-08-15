using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.NewsCategory
{
    public class CategoryUpdateModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
