using Domain.Entities;

namespace Applications.Repositories
{
    public interface IAuditPlanRepository : IGenericRepository<AuditPlan>
    {
        Task<List<AuditPlan>> GetEnableAuditPlans();
        Task<List<AuditPlan>> GetDisableAuditPlans();
        Task<List<AuditPlan>> GetAuditPlanByModuleId(Guid ModuleID);
        Task<List<AuditPlan>> GetAuditPlanByClassId(Guid ClassID);
    }
}
