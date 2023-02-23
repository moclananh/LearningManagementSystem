using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ModuleRepository : GenericRepository<Module>, IModuleRepository
    {
        private readonly AppDBContext _dbContext;

        public ModuleRepository(AppDBContext appDBContext, ICurrentTime currentTime, IClaimService claimService) : base(appDBContext, currentTime, claimService)
        {
            _dbContext = appDBContext;
        }

        public async Task<List<Module>> GetDisableModules() => await _dbContext.Modules.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable).ToListAsync();

        public async Task<List<Module>> GetEnableModules() => await _dbContext.Modules.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable).ToListAsync();

        public async Task<List<Module>> GetModuleByName(string name) => await _dbContext.Modules.Where(x => x.ModuleName.Contains(name)).ToListAsync();

        public async Task<List<Module>> GetModulesBySyllabusId(Guid syllabusId) => await _dbContext.SyllabusModule.Where(s => s.SyllabusId == syllabusId).Select(m => m.Module).ToListAsync();
    }
}
