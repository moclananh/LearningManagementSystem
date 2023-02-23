using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
