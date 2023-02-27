using Applications.Repositories;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IAuditResultRepository : IGenericRepository<AuditResult>
    {
        Task<AuditResult> GetByAuditPlanId(Guid id);
        Task<AuditResult> GetAuditResultById(Guid id);
    }
}
