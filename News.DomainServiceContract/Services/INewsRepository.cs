using News.Freamwork.BaseInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.DomainServiceContract.Services
{
    public interface INewsRepository : BaseInterface<Domain.News.Models.News , int>
    {
        Task<bool> IsNewsExistsAsync(int id);
        Task<bool> IsNewsTitleExistAsync(string name);
        Task<bool> IsNewsSlugExistAsync(string slug);
        Task<bool> IsNewsTitleExistsExceptCurrentAsync(string name, int id);
        Task<bool> IsNewsSlugExistsExceptCurrentAsync(string slug, int id);
        Task<List<Domain.News.Models.News>> GetLatestAsync(int count);

        Task<Domain.News.Models.News?> GetBySlugAsync(string slug);

        Task PublishNewsAsync(int newsId);
        Task AddNewsViewCountAsync(int id, int number);
        Task<bool> IsCategoryExist(int categoryId);



    }
}
