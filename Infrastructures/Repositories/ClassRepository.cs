using Applications.Repositories;
using Domain.Entities;

namespace Infrastructures.Repositories
{
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        private readonly AppDBContext _dbContext;
        public ClassRepository(AppDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Class>> GetDisableClasses()
        {
            throw new NotImplementedException();
        }
        public Task<List<Class>> GetEnableClasses()
        {
            throw new NotImplementedException();
        }
    }
}
