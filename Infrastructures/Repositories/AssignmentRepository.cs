using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
    {
        private readonly AppDBContext _dbContext;
        public AssignmentRepository(AppDBContext dbContext, ICurrentTime currentTime, IClaimService claimService) : base(dbContext,currentTime,claimService)
        {
            _dbContext = dbContext;
        }

        public async Task<Pagination<Assignment>> GetAssignmentByName(string Name, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Assignments.CountAsync();
            var items = await _dbSet.Where(x => x.AssignmentName.Contains(Name))
                                    .OrderByDescending(x => x.CreationDate)
            .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Assignment>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<Assignment>> GetAssignmentByUnitId(Guid UnitId, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Assignments.CountAsync();
            var items = await _dbSet.Where(x => x.UnitId.Equals(UnitId))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Assignment>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };
            return result;
        }

        public async Task<Pagination<Assignment>> GetDisableAssignmentAsync(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Assignments.CountAsync();
            var items = await _dbSet.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Assignment>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<Assignment>> GetEnableAssignmentAsync(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Assignments.CountAsync();
            var items = await _dbSet.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Assignment>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }
    }
}
