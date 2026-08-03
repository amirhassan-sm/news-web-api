using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.News.Models
{
    public class NewsCategory
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<News> News { get; set; } = new();
    }
}
