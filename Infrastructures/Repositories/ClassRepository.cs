using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        private readonly AppDBContext _dbContext;
        public ClassRepository(AppDBContext dbContext, ICurrentTime currentTime, IClaimService claimService) : base(dbContext,currentTime, claimService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Class>> GetClassByName(string Name) => await _dbContext.Classes.Where(x => x.ClassName.Contains(Name)).ToListAsync();
        public async Task<List<Class>> GetDisableClasses() => await _dbContext.Classes.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable).ToListAsync();
        public async Task<List<Class>> GetEnableClasses() => await _dbContext.Classes.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable).ToListAsync();
    }
}
