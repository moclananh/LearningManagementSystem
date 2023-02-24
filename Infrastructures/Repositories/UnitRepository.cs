using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class UnitRepository : GenericRepository<Unit>, IUnitRepository
    {
        private readonly AppDBContext _context;

        public UnitRepository(AppDBContext appDBContext, ICurrentTime currentTime, IClaimService claimService) : base(appDBContext, currentTime, claimService)
        {
            _context = appDBContext;
        }

        public async Task<List<Unit>> GetDisableUnits() => await _context.Units.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable).ToListAsync();

        public async Task<List<Unit>> GetEnableUnits() => await _context.Units.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable).ToListAsync();

        public async Task<List<Unit>> ViewAllUnitByModuleIdAsync(Guid ModuleId)
        {
            return await _context.ModuleUnit.Where(x => x.ModuleId.Equals(ModuleId)).Select(x => x.Unit).ToListAsync();
        }
    }
}
