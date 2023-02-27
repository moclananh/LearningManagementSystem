using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class AuditPlanRepository : GenericRepository<AuditPlan>, IAuditPlanRepository
    {
        private readonly AppDBContext _dBContext;

        public AuditPlanRepository(AppDBContext dBContext, ICurrentTime currentTime, IClaimService claimService) : base(dBContext, currentTime, claimService)
        {
            _dBContext = dBContext;
        }

        public async Task<Pagination<AuditPlan>> GetAuditPlanByClassId(Guid ClassID, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dBContext.AuditPlans.CountAsync();
            var items = await _dbSet.Where(x => x.ClassId.Equals(ClassID))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<AuditPlan>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<AuditPlan?> GetAuditPlanByModuleId(Guid ModuleID) => _dBContext.AuditPlans.FirstOrDefault(x => x.ModuleId.Equals(ModuleID));

        public async Task<Pagination<AuditPlan>> GetAuditPlanByName(string AuditPlanName, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dBContext.AuditPlans.CountAsync();
            var items = await _dbSet.Where(x => x.AuditPlanName.Contains(AuditPlanName))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<AuditPlan>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<AuditPlan>> GetDisableAuditPlans(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dBContext.AuditPlans.CountAsync();
            var items = await _dbSet.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<AuditPlan>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<AuditPlan>> GetEnableAuditPlans(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dBContext.AuditPlans.CountAsync();
            var items = await _dbSet.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<AuditPlan>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

    }
}
