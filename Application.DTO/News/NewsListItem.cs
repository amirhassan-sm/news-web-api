using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.News
{
    public class NewsListItem
    {
        public int NewsId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public string AuthorName { get; set; } = string.Empty;

        public string? ThumbnailUrl { get; set; }

        public DateTime PublishedAt { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public int CommentCount { get; set; }

    }
}
