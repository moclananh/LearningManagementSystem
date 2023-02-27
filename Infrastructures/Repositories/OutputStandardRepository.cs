using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class OutputStandardRepository : GenericRepository<OutputStandard>, IOutputStandardRepository
    {
        private readonly AppDBContext _dbContext;
        public OutputStandardRepository(AppDBContext dbContext,
            ICurrentTime currentTime,
            IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
        }

        public async Task<Pagination<OutputStandard>> GetOutputStandardBySyllabusIdAsync(Guid SyllabusId, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.OutputStandards.CountAsync();
            var items = await _dbContext.SyllabusOutputStandard.Where(x => x.SyllabusId.Equals(SyllabusId))
                                    .Select(x => x.OutputStandard)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<OutputStandard>()
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
