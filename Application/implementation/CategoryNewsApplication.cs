using Application.Common.BaseModel;
using Application.Contrast.QueryServices;
using Application.Contrast.Services;
using Application.DTO.NewsCategory;
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
    public class CategoryNewsApplication : ICategoryNewsApplication
    {
        private readonly INewsCategoryRepository repo;
        private readonly ICategoryNewsQueryService queryService;
        private readonly ILogger<CategoryNewsApplication> logger;
        public CategoryNewsApplication(INewsCategoryRepository repo, ICategoryNewsQueryService queryService, ILogger<CategoryNewsApplication> logger)
        {
            this.repo = repo;
            this.queryService = queryService;
            this.logger = logger;
            
        }
        public async Task<OperationResult> AddNewsCategory(CategoryAddModel model)
        {
            try
            {
                if (await repo.IsCategoryNameExistAsync(model.Name))
                {
                    return 
                        OperationResult.ToFail("add faled ", new List<string> { "this category name already exist" }, "CategoryName_Already_Exist"
                        , System.Net.HttpStatusCode.BadRequest);
                    
                }
                if (await repo.IsCategorySlugExistAsync(model.Slug))
                {
                    return
                        OperationResult.ToFail("add faled ", new List<string> { "this category slug already exist" }, "CategorySlug_Already_Exist"
                        , System.Net.HttpStatusCode.BadRequest);

                }
                var entity = new NewsCategory { 
                    Description = model.Description , 
                    Name = model.Name ,
                    Slug = model.Slug ,
                    
                
                
                };
                await repo.AddAsync(entity);
                return OperationResult.ToSuccess("news Category Added successfullt");


            }
            catch (Exception ex) {
                logger.LogError(ex, "faield to add News category {newsCategoryName}", model.Name);
                return OperationResult.ToFail("failed to add",new List<string> {" an unexpected error occured"}, "Exception_Occured",
                    System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<GenericOperationResult<AllCategoriesListModel>> GetAllCategories(PageModel page)
        {
            try
            {
                return await queryService.getAll(page);




            }
            catch (Exception ex)
            {
                logger.LogError(ex, "faield to Get all categories ");
                return GenericOperationResult<AllCategoriesListModel>.ToFail("Get all categories failed",
                    new List<string> { " an unexpected error occured" }, "Exception_Occured",
                    System.Net.HttpStatusCode.BadRequest);

            }
        }

        public async Task<GenericOperationResult<CategoryUpdateModel>> GetCategoryNewsById(int id)
        {
            try
            {
                var cat = await repo.GetAsync(id);
                if (cat == null)
                {
                    return GenericOperationResult<CategoryUpdateModel>.ToFail("Failed to get category news",
                        new List<string> { "this news categpro does not exist" }, "NewsCategory_Not_Exist", HttpStatusCode.NotFound);


                }
                var model = new CategoryUpdateModel
                {
                    CategoryId = id,
                    Description = cat.Description,
                    Name = cat.Name,
                    Slug = cat.Slug,
                };
                return GenericOperationResult<CategoryUpdateModel>.ToSuccess("get succeed", model);



            }
            catch (Exception ex)
            {
                logger.LogError(ex, "faield to Get category {id} ", id);
                return GenericOperationResult<CategoryUpdateModel>.ToFail("Failed to get category news",
                        new List<string> { "an unexpected error ocured" }, "Exception_Occured", HttpStatusCode.InternalServerError);
            }
        }      

        public async Task<OperationResult> RemoveNewsCategory(int id)
        {
            try
            {
                var cat = await repo.GetAsync(id);
                if (cat == null)
                {
                    return OperationResult.ToFail("failed to remove News Category", new List<string> { "this news category does not exist" }
                    , "NewsCategory_Not_Exist", HttpStatusCode.NotFound);
                    
                }
                if (await repo.NewsCategoryHasChild(id))
                {
                    return OperationResult.ToFail("failed to remove News Category", new List<string> { "this news category has related news" }
               , "NewsCategory_Has_Child", HttpStatusCode.BadRequest);

                }
                await repo.RemoveAsync(id);
                return OperationResult.ToSuccess(id, "remove nwes category suceed");



            }
            catch (Exception ex) {
                logger.LogError(ex, "faield to remove news category {id} ", id);
                return OperationResult.ToFail("Failed to remove category news",
                        new List<string> { "an unexpected error ocured" }, "Exception_Occured", HttpStatusCode.InternalServerError);


            }
        }

        public async Task<GenericComplexResult<CategorySearchModel, CategoryListIteam>> SearchCategory(CategorySearchModel sm)
        {
            try
            {
                return await queryService.SearchCategory(sm);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "failed to search ");
                return new GenericComplexResult<CategorySearchModel, CategoryListIteam>();

            }
        }

        public async Task<OperationResult> UpdateNewsCategory(CategoryUpdateModel model)
        {
            try
            {
                var entity = await repo.GetAsync(model.CategoryId);
                if (entity ==null)
                {
                    return OperationResult.ToFail("failed to update", new List<string> { "this news categor does not exist" }, "NewsCategory_Not_Exist",
                        HttpStatusCode.NotFound);
                    
                }
                if (await repo.IsCategoryNameExistExceptCurrentAsync(model.CategoryId,model.Name))
                {
                    return OperationResult.ToFail("failed to update", new List<string> { "this news category name already exists" },
                        "NewsCategoryName_Already_Exist",
                       HttpStatusCode.BadRequest);

                }
                if (await repo.IsCategorySlugExistExceptCurrentAsync(model.CategoryId, model.Slug))
                {
                    return OperationResult.ToFail("failed to update", new List<string> { "this news category slug already exists" },
                        "NewsCategorySlug_Already_Exist",
                       HttpStatusCode.BadRequest);

                }
                entity.Description = model.Description;
                entity.Name = model.Name;
                entity.Slug=model.Slug;
                await repo.UpdateAsync(entity);
                return OperationResult.ToSuccess(model.CategoryId, "category updated successfully");


            }
            catch(Exception ex)
            {
                logger.LogError(ex, "faield to update news category {id} ", model.CategoryId);
                return OperationResult.ToFail("failed to update", new List<string> { "an unexpected error occured" }, "Exception_Occured",
                        HttpStatusCode.InternalServerError);

            }
        }
    }
}
