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
        private readonly AppDBContext _context;

        public AuditResultRepository(AppDBContext dbContext, ICurrentTime currentTime, IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _context = dbContext;
        }

        public async Task<AuditResult> GetAuditResultById(Guid id)
        {
            return await _context.AuditResults.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AuditResult> GetByAuditPlanId(Guid id)
        {
            return await _context.AuditResults.FirstOrDefaultAsync(x => x.AuditPlanId == id);
        }
    }
}
