using Applications.Interfaces;
using Applications.Repositories;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ModuleUnitRepository : GenericRepository<ModuleUnit>, IModuleUnitRepository
    {
        private readonly AppDBContext _dbContext;
        public ModuleUnitRepository(AppDBContext appDBContext, ICurrentTime currentTime, IClaimService claimService) : base(appDBContext, currentTime, claimService)
        {
            _dbContext = appDBContext;
        }

        public async Task<ModuleUnit> GetModuleUnit(Guid ModuleId, Guid UnitId)
        {
            return await _dbContext.ModuleUnit.FirstOrDefaultAsync(x => x.ModuleId== ModuleId && x.UnitId == UnitId);
        }
    }
}
