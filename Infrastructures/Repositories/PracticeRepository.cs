using Applications.Commons;
using Applications.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class PracticeRepository : GenericRepository<Practice>, IPracticeRepository
    {
        private readonly AppDBContext _dbContext;

        public PracticeRepository(AppDBContext appDBContext, ICurrentTime currentTime, IClaimService claimService) : base(appDBContext, currentTime, claimService)
        {
            _dbContext = appDBContext;
        }

        public async Task<Practice> GetByPracticeId(Guid PracticeId)
        {
            return await _dbContext.Practices.FirstOrDefaultAsync(x => x.Id == PracticeId);
        }

        public async Task<Pagination<Practice>> GetPracticeByUnitId(Guid UnitId, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Practices.CountAsync();
            var items = await _dbContext.Practices.Where(x => x.UnitId.Equals(UnitId))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Practice>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };
            return result;
        }
        public async Task<Pagination<Practice>> GetPracticeByName(string Name, int pageNumber = 0, int pageSize = 10)
        {

            var itemCount = await _dbContext.Practices.CountAsync();
            var items = await _dbContext.Practices.Where(x => x.PracticeName.Contains(Name))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Practice>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }
        public async Task<Pagination<Practice>> GetDisablePractices(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Practices.CountAsync();
            var items = await _dbContext.Practices.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<Practice>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };
            return result;
        }
        public async Task<Pagination<Practice>> GetEnablePractices(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Practices.CountAsync();
            var items = await _dbContext.Practices.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<Practice>()
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
