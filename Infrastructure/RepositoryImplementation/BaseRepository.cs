using Infrastructure.RepositoryAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoryImplementation
{
    public abstract class BaseRepository<TModel, TKey> : IBaseRepository<TModel, TKey> where TModel : class
    {
        protected BaseRepository(AppDBContext context)
        {
            Context = context;
        }
        protected AppDBContext Context { get; set; }
        protected DbSet<TModel> ModelSet => Context.Set<TModel>();
        public abstract System.Linq.Expressions.Expression<Func<TModel, TKey>> GetKey();

        public async virtual Task<TModel> Add(TModel item)
        {

            ModelSet.Add(item);
            await Context.SaveChangesAsync();
            return item;
        }

        public async virtual Task<TModel> Update(TModel item)
        {

            Context.Entry(item).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            
            return item;
        }

        public async virtual Task<TModel> Delete(TModel item)
        {
            ModelSet.Remove(item);
            Context.SaveChanges();
            await Context.SaveChangesAsync();

            return item;
        }
        public virtual IQueryable<TModel> Items()
        {
            return ModelSet;
        }
        public virtual IQueryable<TModel> Items(int PageSize = 0, int PageIndex = 0)
        {
            return ModelSet.OrderBy(GetKey()).Skip(PageSize * PageIndex).Take(PageSize);
        }

        public async Task<IEnumerable<TModel>> AddRange(IEnumerable<TModel> items)
        {
            ModelSet.AddRange(items);
            Context.SaveChanges();
            await Context.SaveChangesAsync();

            return items;
        }
    }
}
