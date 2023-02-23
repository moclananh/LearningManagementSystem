using Applications.ViewModels.AuditPlanViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces
{
    public interface IAuditPlanService
    {
        public Task<List<AuditPlanViewModel>> GetAllAuditPlanAsync();
        public Task<List<AuditPlanViewModel>> GetEnableAuditPlanAsync();
        public Task<List<AuditPlanViewModel>> GetDisableAuditPlanAsync();
        public Task<AuditPlanViewModel> GetAuditPlanByIdAsync(Guid AuditPlanId);
        public Task<List<AuditPlanViewModel>> GetAuditPlanByModuleIdAsync(Guid ModuleId);
        public Task<List<AuditPlanViewModel>> GetAuditPlanbyClassIdAsync(Guid ClassId);
        public Task<AuditPlanViewModel?> CreateAuditPlanAsync(AuditPlanViewModel AuditPlanDTO);
        public Task<UpdateAuditPlanViewModel?> UpdateAuditPlanAsync(Guid auditPlanId, UpdateAuditPlanViewModel updateAuditPlanView);
    }
}
