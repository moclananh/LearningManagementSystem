using Applications.ViewModels.AuditResultViewModels;

namespace Application.Interfaces
{
    public interface IAuditResultServices
    {
        Task<AuditResultViewModel> GetByAudiPlanId(Guid id);
        Task<AuditResultViewModel> GetAuditResultById(Guid Id);
        Task<UpdateAuditResultViewModel> UpdateAuditResult(Guid AuditResultId, UpdateAuditResultViewModel classDTO);
    }
}
