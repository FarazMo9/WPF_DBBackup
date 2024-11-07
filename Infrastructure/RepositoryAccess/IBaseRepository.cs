using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryAccess
{
    public interface IBaseRepository<TModel, TKey>
    {
        public Task<TModel> Add(TModel item);
        public Task<TModel> Update(TModel item);
        public Task<TModel> Delete(TModel item);
        public IQueryable<TModel> Items();
        public IQueryable<TModel> Items(int PageSize = 0, int PageIndex = 0);
        public Task<IEnumerable<TModel>> AddRange(IEnumerable<TModel> items);

    }
}
