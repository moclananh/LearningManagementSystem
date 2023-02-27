using Applications.Interfaces;
using Applications.Repositories;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class UserAuditPlanRepository : GenericRepository<UserAuditPlan>, IUserAuditPlanRepository
    {
        private readonly AppDBContext _dbContext;
        public UserAuditPlanRepository(AppDBContext appDBContext, ICurrentTime currentTime, IClaimService claimService) : base(appDBContext, currentTime, claimService)
        {
            _dbContext = appDBContext;
        }
        public async Task<UserAuditPlan> GetUserAuditPlan(Guid AuditPLanId, Guid UserId) => await _dbContext.UserAuditPlan.FirstOrDefaultAsync(x => x.AuditPlanId == AuditPLanId && x.UserId == UserId);
    }
}
