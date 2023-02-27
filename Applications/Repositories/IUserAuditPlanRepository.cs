using Domain.EntityRelationship;

namespace Applications.Repositories
{
    public interface IUserAuditPlanRepository : IGenericRepository<UserAuditPlan>
    {
        Task<UserAuditPlan> GetUserAuditPlan(Guid AuditPLanId, Guid UserId);
    }
}
