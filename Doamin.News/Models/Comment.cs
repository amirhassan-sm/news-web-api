using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Domain.News.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int NewsId{ get; set; }
        public int UserId { get; set; }
        public int ParentCommentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsAproved { get; set; }

        public DateTime CreatedAt { get; set; }


        public News News { get; set; } = new();



    }


}
