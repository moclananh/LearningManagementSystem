using Applications.Interfaces;
using Applications.Repositories;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class SyllabusModuleRepository : GenericRepository<SyllabusModule>, ISyllabusModuleRepository
    {
        private readonly AppDBContext _dbContext;
        public SyllabusModuleRepository(AppDBContext appDBContext, ICurrentTime currentTime, IClaimService claimService) : base(appDBContext, currentTime, claimService)
        {
            _dbContext = appDBContext;
        }

        public async Task<SyllabusModule> GetSyllabusModule(Guid SyllabusId, Guid ModuleId)
        {
            return await _dbContext.SyllabusModule.FirstOrDefaultAsync(x => x.SyllabusId == SyllabusId && x.ModuleId == ModuleId);
        }
    }
}
