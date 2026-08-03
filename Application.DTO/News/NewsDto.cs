using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.News
{
    public class NewsDto
    {
        public int NewsId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;
        public string Summery { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
        public int AuthorId { get; set; }

        public DateTime PublishedDate { get; set; }
        public DateTime CraetedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool Status { get; set; }
        public int ViewCount { get; set; }

        public string Metatag { get; set; } = string.Empty;
        public string MedtaData { get; set; } = string.Empty;
        public string Metaescription { get; set; } = string.Empty;

    }
}
