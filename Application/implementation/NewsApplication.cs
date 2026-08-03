using Application.Common.BaseModel;
using Application.Contrast.QueryService;
using Application.Contrast.Services;
using Application.DTO.News;
using Application.FreamWork.OperatonResult;
using Application.FreamWork.SearchBaseModel;
using Domain.News.Models;
using Microsoft.Extensions.Logging;
using News.DomainServiceContract.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.implementation
{
    public class NewsApplication : INewsApplication
    {
        private readonly INewsQuery newsQuery;
        private readonly INewsRepository repo;
        private readonly ILogger<NewsApplication> logger;
        public NewsApplication(INewsQuery newsQuery, INewsRepository repo, ILogger<NewsApplication> logger)
        {
            this.newsQuery = newsQuery;
            this.repo = repo;
            this.logger = logger;

        }

        private NewsDto MapEntityToDto(Domain.News.Models.News item)
        {
            return new NewsDto
            {
                AuthorId = item.AuthorId,
                CategoryId = item.CategoryId,
                Content = item.Content,
                CraetedAt = item.CraetedAt,
                MedtaData = item.MedtaData,
                Metaescription = item.Metaescription,
                Metatag = item.Metatag,
                NewsId = item.NewsId,
                PublishedDate = item.PublishedDate.Value,
                Slug = item.Slug,
                Status = item.Status,
                Summery = item.Summery,
                Title = item.Title,
                UpdatedAt = item.UpdatedAt.Value,
                ViewCount = item.ViewCount,
            };

        }


        public async Task<OperationResult> AddAsync(NewsAddDto model, int AuthorId)
        {
            try
            {
                if (!await repo.IsCategoryExist(model.CategoryId))
                {
                    return OperationResult
                        .ToFail("Failed to add", new List<string> { "this category does not exist" }, "Category_NotExist",
                        HttpStatusCode.NotFound);

                }
                if (await repo.IsNewsTitleExistAsync(model.Title))
                {
                    return OperationResult
                        .ToFail("Failed to add", new List<string> { "this news title already exist" }, "NewsTitle_Already_Exist",
                        HttpStatusCode.Conflict);

                }
                if (await repo.IsNewsSlugExistAsync(model.Slug))
                {
                    return OperationResult
                        .ToFail("Failed to add", new List<string> { "this news slug already exist" }, "slug_Already_Exist",
                        HttpStatusCode.Conflict);

                }







                var dbModel = new Domain.News.Models.News
                {
                    AuthorId = AuthorId,
                    CategoryId = model.CategoryId,
                    Content = model.Content,
                    Title = model.Title,
                    Slug = model.Slug,
                    Summery = model.Summary,
                    Status = model.Status,


                };
                await repo.AddAsync(dbModel);
                return OperationResult.ToSuccess("News Added successfully");




            }
            catch (Exception ex)
            {
                logger.LogError(ex,
    "Failed to add news. Title: {Title}, CategoryId: {CategoryId}, AuthorId: {AuthorId}",
    model.Title,
    model.CategoryId,
    AuthorId);

                return OperationResult.ToFail("failed to add", new List<string> { "an unexpected error occured" }, "Exception_Occured"
                    , HttpStatusCode.InternalServerError);


            }
        }
       

        public async Task<GenericOperationResult<NewsDto>> GetAsync(int id)
        {
            try
            {
                if (!await repo.IsNewsExistsAsync(id))
                {
                    return GenericOperationResult<NewsDto>.ToFail(id, "get failed", new List<string> { "this news  does not exist" }, "News_Not_Exist"
                        , HttpStatusCode.NotFound);

                }
                await repo.AddNewsViewCountAsync(id,1);
                var item = await repo.GetAsync(id);
                
                var model = MapEntityToDto(item);

                return GenericOperationResult<NewsDto>.ToSuccess("get suceede", model);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed to get the news {id}", id);
                return GenericOperationResult<NewsDto>.ToFail(id, "get failed",
                    new List<string> { "an unexpected error occured" }, "Exception_Ocurred"
                       , HttpStatusCode.InternalServerError);

            }
        }
       

        public async Task<GenericOperationResult<NewsDto>> GetBySlugAsync(string slug)
        {
            try
            {
                var entity = await repo.GetBySlugAsync(slug);
                if (entity == null)
                {
                    return GenericOperationResult<NewsDto>
                        .ToFail("get failed", new List<string> { "this news  does not exist" }, "News_Not_Exist"
                         , HttpStatusCode.NotFound);

                }

                var model = MapEntityToDto(entity);
                await repo.AddNewsViewCountAsync(model.NewsId, 1);
                return GenericOperationResult<NewsDto>.ToSuccess("get suceede", model);


            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed to get the news {slug}", slug);
                return GenericOperationResult<NewsDto>.ToFail("get failed",
                    new List<string> { "an unexpected error occured" }, "Exception_Ocurred"
                       , HttpStatusCode.InternalServerError);

            }
        }
        //todo fill author name here
        //todo fill category name here
        //todo fill comment count  here
        //todo fill ThumbnailUrl  here

        public async Task<GenericOperationResult<List<NewsListItem>>> GetLatestAsync(int count)
        {
            try
            {
                if (count <= 0)
                {
                    return GenericOperationResult<List<NewsListItem>>.ToFail("get failed",
                   new List<string> { "count can not be zero or negetive " }, "Count_IsLessThan_one"
                      , HttpStatusCode.BadRequest);

                }

                //to do 
                var entityList = await repo.GetLatestAsync(count);
                var model = entityList.Select(x => new NewsListItem
                {
                    AuthorName = "todo",
                    CategoryName = "todo",
                    CommentCount = 0,
                    NewsId = x.NewsId,
                    PublishedAt = x.PublishedDate.Value,
                    Summary = x.Summery,
                    ThumbnailUrl = "todo",
                    Title = x.Title,
                }).ToList();

                return GenericOperationResult<List<NewsListItem>>.ToSuccess("get succeed", model);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed to get the news {count}", count);
                return GenericOperationResult<List<NewsListItem>>.ToFail("get failed",
                    new List<string> { "an unexpected error occured" }, "Exception_Ocurred"
                       , HttpStatusCode.InternalServerError);

            }
        }

        public async Task<OperationResult> PublishNewsAsync(int newsId)
        {
            try
            {
                if (!await repo.IsNewsExistsAsync(newsId))
                {
                    return OperationResult.ToFail(newsId, "failed to publish", new List<string> { "this news does not exist" }
                    , "News_Not_Exist", HttpStatusCode.NotFound);

                }
                await repo.PublishNewsAsync(newsId);
                return OperationResult.ToSuccess(newsId, "news published succeefully");



            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed to get the news {newsId}", newsId);
                return OperationResult.ToFail("get failed",
                    new List<string> { "an unexpected error occured" }, "Exception_Ocurred"
                       , HttpStatusCode.InternalServerError);



            }
        }
        //todo remove its media and comments like ....

        public async Task<OperationResult> RemoveAsync(int id)
        {
            try
            {
                if (!await repo.IsNewsExistsAsync(id))
                {
                    return OperationResult.ToFail(id, "failed to remove news", new List<string> { "this news does not exist" }
                   , "News_Not_Exist", HttpStatusCode.NotFound);


                }

                await repo.RemoveAsync(id);
                return OperationResult.ToSuccess(id, "news Removed succeefully");



            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed to get the news {newsId}", id);
                return OperationResult.ToFail("get failed",
                    new List<string> { "an unexpected error occured" }, "Exception_Ocurred"
                       , HttpStatusCode.InternalServerError);



            }
        }

        public async Task<GenericComplexResult<NewsSearchModel, NewsListItem>> Search(NewsSearchModel sm)
        {
            try
            {
                return await newsQuery.Search(sm);

            }
            catch (Exception ex) {
                logger.LogError(ex, "failed search {Phrase}{CategoryId}", sm.Phrase , sm.CategoryId);
                return new GenericComplexResult<NewsSearchModel, NewsListItem>();



            }
        }

        public async Task<OperationResult> UpdateAsync(NewsUpdateDto model, int AuthorId)
        {
            try
            {
                if (!await repo.IsNewsExistsAsync(model.NewsId))
                {
                    return OperationResult.ToFail(model.NewsId, "failed to update news", new List<string> { "this news does not exist" }
                  , "News_Not_Exist", HttpStatusCode.NotFound);


                }
                var entity = new Domain.News.Models.News
                {
                    AuthorId=AuthorId,
                    CategoryId=model.CategoryId,
                    NewsId=model.NewsId,
                    Content = model.Content,
                    MedtaData = model.MedtaData,
                    Metaescription = model.Metaescription,
                    Metatag = model.Metatag,
                    Slug = model.Slug,
                    Status = model.Status,
                    Summery = model.Summery,
                    Title = model.Title,  
                };

                await repo.UpdateAsync(entity);
                return OperationResult.ToSuccess(model.NewsId,"updated successfully");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed to get the news {newsId}", model.NewsId);
                return OperationResult.ToFail("update failed",
                    new List<string> { "an unexpected error occured" }, "Exception_Ocurred"
                       , HttpStatusCode.InternalServerError);

            }
        }
    }
}
