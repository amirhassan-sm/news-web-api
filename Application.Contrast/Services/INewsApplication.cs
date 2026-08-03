using Application.Common.BaseModel;
using Application.DTO.News;
using Application.FreamWork.OperatonResult;
using Application.FreamWork.SearchBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contrast.Services
{
    public interface INewsApplication
    {
        Task<GenericOperationResult<List<NewsListItem>>> GetLatestAsync(int count);

        Task<GenericOperationResult<NewsDto>> GetBySlugAsync(string slug);

        Task<OperationResult> PublishNewsAsync(int newsId);

        Task<OperationResult> AddAsync(NewsAddDto model, int AuthorId);
        Task<OperationResult> RemoveAsync(int id);

        Task<OperationResult> UpdateAsync(NewsUpdateDto model, int AuthorId);

        Task<GenericOperationResult<NewsDto>> GetAsync(int id);
        public Task<GenericComplexResult<NewsSearchModel, NewsListItem>> Search(NewsSearchModel sm);
    }
}
