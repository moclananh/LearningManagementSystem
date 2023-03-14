using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected DbSet<TEntity> _dbSet;
        private ICurrentTime _timeService;
        private IClaimService _claimService;

        public GenericRepository(AppDBContext appDBContext,
            ICurrentTime currentTime,
            IClaimService claimService) // contructor 3 param
        {
            _dbSet = appDBContext.Set<TEntity>();
            _timeService = currentTime;
            _claimService = claimService;
        }

        public async Task AddAsync(TEntity entity)
        {
            entity.CreationDate = DateTime.UtcNow;
            entity.CreatedBy = _claimService.GetCurrentUserId;
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreationDate = DateTime.UtcNow;
                entity.CreatedBy = _claimService.GetCurrentUserId;
            }
            await _dbSet.AddRangeAsync(entities);
        }

        public Task<List<TEntity>> GetAllAsync() => _dbSet.ToListAsync();

        public async Task<TEntity?> GetByIdAsync(Guid? id)
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public void SoftRemove(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletionDate = _timeService.CurrentTime();
            entity.DeleteBy = _claimService.GetCurrentUserId;
            _dbSet.Update(entity);
        }

        public void SoftRemoveRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.DeletionDate = _timeService.CurrentTime();
                entity.DeleteBy = _claimService.GetCurrentUserId;
            }
            _dbSet.UpdateRange(entities);
        }

        public void Update(TEntity entity)
        {
            entity.ModificationDate = _timeService.CurrentTime();
            entity.ModificationBy = _claimService.GetCurrentUserId;
            _dbSet.Update(entity);
        }

        public void UpdateRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreationDate = DateTime.UtcNow;
                entity.CreatedBy = _claimService.GetCurrentUserId;
            }
            _dbSet.UpdateRange(entities);
        }
        public async Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> expression)
        {
            var data = await _dbSet.Where(expression).ToListAsync();
            return data;
        }
        public IQueryable<TEntity> query()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<Pagination<TEntity>> ToPagination(int pageIndex = 0, int pageSize = 10)
        {
            var itemCount = await _dbSet.CountAsync();
            var items = await _dbSet.OrderByDescending(x => x.CreationDate)
                                    .Skip(pageIndex * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<TEntity>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public void Approve(TEntity entity)
        {
            entity.ApprovedDate = _timeService.CurrentTime();
            entity.ApprovedBy = _claimService.GetCurrentUserId;
            _dbSet.Update(entity);
        }

        public async Task<List<TEntity>> GetEntitiesByIdsAsync(List<Guid?> Ids) => await _dbSet.Where(x => Ids.Contains(x.Id)).ToListAsync();
    }
}
