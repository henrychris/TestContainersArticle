using System.Linq.Expressions;

namespace TestContainersArticle.Main.Data.Repositories.Base
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(string id);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        IQueryable<TEntity> GetQueryable();
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<List<TEntity>> GetEntitiesByFilter(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllAsync();
    }
}
