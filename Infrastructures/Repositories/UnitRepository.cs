using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructures.Repositories
{
    public class UnitRepository : GenericRepository<Unit>, IUnitRepository
    {
        private readonly AppDBContext _dbContext;
        public UnitRepository(AppDBContext appDBContext, ICurrentTime currentTime, IClaimService claimService) : base(appDBContext, currentTime, claimService)
        {
            _dbContext = appDBContext;
        }

        public async Task<Pagination<Unit>> GetDisableUnits(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Units.CountAsync();
            var items = await _dbContext.Units.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Unit>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<Unit>> GetEnableUnits(int pageNumber = 0, int pageSize = 10) 
        {
            var itemCount = await _dbContext.Units.CountAsync();
            var items = await _dbContext.Units.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Unit>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<Unit>> ViewAllUnitByModuleIdAsync(Guid ModuleId, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Units.CountAsync();
            var items = await _dbContext.ModuleUnit.Where(x => x.ModuleId.Equals(ModuleId))
                                    .Select(x => x.Unit)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Unit>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<Unit>> GetUnitByNameAsync(string UnitName, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Units.CountAsync();
            var items = await _dbContext.Units.Where(x => x.UnitName.Contains(UnitName))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Unit>()
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

