using Application.DTO.News;
using Application.FreamWork.SearchBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contrast.QueryService
{
    public interface INewsQuery
    {
        public Task<GenericComplexResult<NewsSearchModel,NewsListItem>> Search(NewsSearchModel sm);

    }
}
