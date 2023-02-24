using Applications.Commons;
using Domain.Entities;

namespace Applications.Repositories
{
    public interface IAuditPlanRepository : IGenericRepository<AuditPlan>
    {
        Task<Pagination<AuditPlan>> GetEnableAuditPlans(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<AuditPlan>> GetDisableAuditPlans(int pageNumber = 0, int pageSize = 10);
        Task<AuditPlan?> GetAuditPlanByModuleId(Guid ModuleID);
        Task<Pagination<AuditPlan>> GetAuditPlanByClassId(Guid ClassID, int pageNumber = 0, int pageSize = 10);
        Task<Pagination<AuditPlan>> GetAuditPlanByName(string AuditPlanName, int pageNumber = 0, int pageSize = 10);
    }
}
