using System;
using System.Collections.Generic;
using System.Text;
using Application.Contrast.QueryService;
using Application.DTO.News;
using Application.FreamWork.SearchBaseModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.News.Ef.Persistance.Query
{
    public class NewsQuery : INewsQuery
    {
        private readonly NewsContext db;
        public NewsQuery(NewsContext db)
        {
            this.db = db;
            
        }
        public async Task<GenericComplexResult<NewsSearchModel, NewsListItem>> Search(NewsSearchModel sm)
        {

            var q = from item in db.News select item;
            q = q.AsNoTracking();

            if (!string.IsNullOrEmpty(sm.Phrase))
            {
                q = q.Where(x => x.Title.Contains(sm.Phrase) || x.Summery.Contains(sm.Phrase));
                
            }
            if (sm.CategoryId !=null)
            {
                q = q.Where(x => x.CategoryId == sm.CategoryId);
                
            }
           

            sm.RecordCount = await q.CountAsync();

            var q2 =  q.Select( x=> new NewsListItem
            {
                //todo fill it

                AuthorName = "Todo",
                Title = x.Title,
                //todo fill it

                CategoryName = "",
                //todo fill it
                CommentCount = 0,
                NewsId=x.NewsId,
                PublishedAt = x.PublishedDate.Value,
                Summary = x.Summery,
                ThumbnailUrl = "todo"

            }
            );
            q2 =  q2.OrderByDescending(x => x.NewsId).Skip(sm.pageIndex - 1 * sm.pageSize).Take(sm.pageSize);
            GenericComplexResult<NewsSearchModel, NewsListItem> result = new();
            result.SearchModel=sm;
            result.ListIteams = await q2.ToListAsync();

            return result;







        }
    }
}
