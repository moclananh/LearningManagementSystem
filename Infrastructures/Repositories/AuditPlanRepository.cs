using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class AuditPlanRepository : GenericRepository<AuditPlan>, IAuditPlanRepository
    {
        private readonly AppDBContext _dBContext;

        public AuditPlanRepository(AppDBContext dBContext, ICurrentTime currentTime, IClaimService claimService) : base(dBContext, currentTime, claimService)
        {
            _dBContext = dBContext;
        }

        public async Task<List<AuditPlan>> GetAuditPlanByClassId(Guid ClassID)
        {
            return await _dBContext.AuditPlans.Where(x => x.ClassId.Equals(ClassID)).ToListAsync();
        }

        public async Task<List<AuditPlan>> GetAuditPlanByModuleId(Guid ModuleID)
        {
            return await _dBContext.AuditPlans.Where(x => x.ModuleId.Equals(ModuleID)).ToListAsync();
        }

        public async Task<List<AuditPlan>> GetDisableAuditPlans()
        {
            return await _dBContext.AuditPlans.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable).ToListAsync();
        }

        public async Task<List<AuditPlan>> GetEnableAuditPlans()
        {
            return await _dBContext.AuditPlans.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable).ToListAsync();
        }
    }
}
