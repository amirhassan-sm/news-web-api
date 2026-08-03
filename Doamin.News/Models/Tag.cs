using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.News.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; }=string.Empty;

         public List<NewsTag> NewsTags { get; set; }=new List<NewsTag>();
    }
}
