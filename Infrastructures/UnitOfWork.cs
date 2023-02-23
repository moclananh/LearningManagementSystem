using Applications;
using Applications.Repositories;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _appDBContext;
        private readonly IClassRepository _classRepository;
        public UnitOfWork(AppDBContext appDBContext,
            IClassRepository classRepository)
        {
            _appDBContext = appDBContext;
            _classRepository = classRepository;
        }

        public IClassRepository ClassRepository => _classRepository;

        public async Task<int> SaveChangeAsync() => await _appDBContext.SaveChangesAsync();
    }
}
