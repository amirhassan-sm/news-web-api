using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.News.Models
{
    public class NewsMedia
    {
        public int NewsMediaId{ get; set; }
        public int NewsId  { get; set; }
        public string Url { get; set; } = string.Empty;

        public int MediaTypeId { get; set; }

        public int DisplayOrder { get; set; }
        public string AltText { get; set; } = string.Empty;
        public bool IsThumbnail { get; set; }

        public News News { get; set; } = new();



    }
}
