using Applications.Interfaces;
using Applications.Repositories;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class SyllabusModulesRepository : GenericRepository<SyllabusModule>, ISyllabusModulesRepository
    {
        private readonly AppDBContext _dbContext;
        public SyllabusModulesRepository(AppDBContext appDBContext, ICurrentTime currentTime, IClaimService claimService) : base(appDBContext, currentTime, claimService)
        {
            _dbContext = appDBContext;
        }
        public async Task<SyllabusModule> GetSyllabusModules(Guid SyllabusId, Guid ModuleId) => await _dbContext.SyllabusModule.FirstOrDefaultAsync(x => x.SyllabusId == SyllabusId && x.ModuleId == ModuleId);
    }
}
