using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.News
{
    public class NewsAddDto
    {
        public int CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public bool Status { get; set; }
    }
}
