using Applications.Commons;
using Applications.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class PracticeRepository : GenericRepository<Practice>, IPracticeRepository
    {
        private readonly AppDBContext _context;

        public PracticeRepository(AppDBContext dbContext, ICurrentTime currentTime, IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _context = dbContext;
        }

        public async Task<Practice> GetByPracticeId(Guid PracticeId)
        {
            return await _context.Practices.FirstOrDefaultAsync(x => x.Id == PracticeId);
        }

        public async Task<Pagination<Practice>> GetPracticeByUnitId(Guid UnitId, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _context.Practices.CountAsync();
            var items = await _dbSet.Where(x => x.UnitId.Equals(UnitId))
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
