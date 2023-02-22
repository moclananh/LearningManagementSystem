using Applications;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _appDBContext;
        public UnitOfWork(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public async Task<int> SaveChangeAsync() => await _appDBContext.SaveChangesAsync();
    }
}
