using Applications.Commons;
using Applications.ViewModels.AuditPlanViewModel;
using Applications.ViewModels.Response;
using Applications.ViewModels.UserAuditPlanViewModels;
using System.Drawing.Printing;

namespace Applications.Interfaces
{
    public interface IAuditPlanService
    {
        public Task<Response> GetAllAuditPlanAsync(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetEnableAuditPlanAsync(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetDisableAuditPlanAsync(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetAuditPlanByIdAsync(Guid AuditPlanId);
        public Task<Response> GetAuditPlanByModuleIdAsync(Guid ModuleId);
        public Task<Response> GetAuditPlanbyClassIdAsync(Guid ClassId, int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetAuditPlanByName(string AuditPlanName, int pageIndex = 0, int pageSize = 10);
        public Task<CreateAuditPlanViewModel?> CreateAuditPlanAsync(CreateAuditPlanViewModel AuditPlanDTO);
        public Task<UpdateAuditPlanViewModel?> UpdateAuditPlanAsync(Guid auditPlanId, UpdateAuditPlanViewModel updateAuditPlanView);
        public Task<CreateUserAuditPlanViewModel> AddUserToAuditPlan(Guid AuditPlanId, Guid UserId);
        public Task<CreateUserAuditPlanViewModel> RemoveUserFromAuditPlan(Guid AuditPlanId, Guid UserId);
        public Task<Response> GetAllUserAuditPlanAsync(int pageIndex = 0, int pageSize = 10);
    }
}
