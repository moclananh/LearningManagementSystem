using Applications.Commons;
using Domain.Base;
using System.Linq.Expressions;

namespace Applications.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<List<TEntity>> GetEntitiesByIdsAsync(List<Guid?> Ids);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(Guid? id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Approve(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        void SoftRemove(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        void SoftRemoveRange(List<TEntity> entities);
        Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> query();
        Task<Pagination<TEntity>> ToPagination(int pageNumber = 0, int pageSize = 10);
    }
}
