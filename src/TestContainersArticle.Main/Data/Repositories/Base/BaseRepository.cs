using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace TestContainersArticle.Main.Data.Repositories.Base
{
    public class BaseRepository<TEntity>(DataContext context) : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DataContext Context = context;

        public virtual async Task AddAsync(TEntity entity)
        {
            await Context.AddAsync(entity);
        }

        public virtual async Task<TEntity?> GetByIdAsync(string id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public virtual void Update(TEntity entity)
        {
            Context.Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            Context.Remove(entity);
        }

        public virtual IQueryable<TEntity> GetQueryable()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            Context.UpdateRange(entities);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.AddRangeAsync(entities);
        }

        public async Task<List<TEntity>> GetEntitiesByFilter(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }
    }
}
