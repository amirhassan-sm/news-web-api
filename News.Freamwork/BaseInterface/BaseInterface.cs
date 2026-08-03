using System;
using System.Collections.Generic;
using System.Text;

namespace News.Freamwork.BaseInterface
{
    public interface BaseInterface<TModel,TKey >
    {
        Task AddAsync(TModel model);
        Task RemoveAsync(TKey id);

        Task UpdateAsync(TModel model);

        Task<TModel?> GetAsync(TKey id);

    }
}
