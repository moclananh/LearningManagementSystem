using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class AbsentRequestRepository : GenericRepository<AbsentRequest>, IAbsentRequestRepository
    {
        private readonly AppDBContext _dbContext;
        public AbsentRequestRepository(AppDBContext dbContext, ICurrentTime currentTime, IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
        }
        public async Task<Pagination<AbsentRequest>> GetAllAbsentRequestByEmail(string Email, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.AbsentRequests.CountAsync();
            var items = await _dbContext.AbsentRequests.Where(x => x.User.Email == Email)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<AbsentRequest>()
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
