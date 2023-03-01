﻿using Applications.Commons;
using Applications.ViewModels.AuditPlanViewModel;
using Applications.ViewModels.UserAuditPlanViewModels;
using System.Drawing.Printing;

namespace Applications.Interfaces
{
    public interface IAuditPlanService
    {
        public Task<Pagination<AuditPlanViewModel>> GetAllAuditPlanAsync(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<AuditPlanViewModel>> GetEnableAuditPlanAsync(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<AuditPlanViewModel>> GetDisableAuditPlanAsync(int pageIndex = 0, int pageSize = 10);
        public Task<AuditPlanViewModel> GetAuditPlanByIdAsync(Guid AuditPlanId);
        public Task<AuditPlanViewModel> GetAuditPlanByModuleIdAsync(Guid ModuleId);
        public Task<Pagination<AuditPlanViewModel>> GetAuditPlanbyClassIdAsync(Guid ClassId, int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<AuditPlanViewModel>> GetAuditPlanByName(string AuditPlanName, int pageIndex = 0, int pageSize = 10);
        public Task<AuditPlanViewModel?> CreateAuditPlanAsync(AuditPlanViewModel AuditPlanDTO);
        public Task<UpdateAuditPlanViewModel?> UpdateAuditPlanAsync(Guid auditPlanId, UpdateAuditPlanViewModel updateAuditPlanView);
        public Task<CreateUserAuditPlanViewModel> AddUserToAuditPlan(Guid AuditPlanId, Guid UserId);
        public Task<CreateUserAuditPlanViewModel> RemoveUserToAuditPlan(Guid AuditPlanId, Guid UserId);
        public Task<Pagination<UserAuditPlanViewModel>> GetAllUserAuditPlanAsync(int pageIndex = 0, int pageSize = 10);
    }
}
