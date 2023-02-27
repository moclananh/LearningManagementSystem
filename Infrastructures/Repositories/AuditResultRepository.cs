using Application.Repositories;
using Applications.Interfaces;
using Domain.Entities;
using Infrastructures;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuditResultRepository : GenericRepository<AuditResult> ,IAuditResultRepository
    {
        private readonly AppDBContext _dbContext;

        public AuditResultRepository(AppDBContext dbContext, ICurrentTime currentTime, IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
        }

        public async Task<AuditResult> GetAuditResultById(Guid id)
        {
            return await _dbContext.AuditResults.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AuditResult> GetByAuditPlanId(Guid id)
        {
            return await _dbContext.AuditResults.FirstOrDefaultAsync(x => x.AuditPlanId == id);
        }
    }
}
