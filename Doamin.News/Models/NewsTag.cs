using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.News.Models
{
    public class NewsTag
    {
        public int NewsTagId { get; set; }
        public int NewsId { get; set; }
        public int TagId { get; set; }

        public News News { get; set; }= new();
        public Tag Tag { get; set; }= new();







    }
}
