using Applications;
using Applications.IRepositories;
using Applications.Repositories;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _appDBContext;
        private readonly IClassRepository _classRepository;
        private readonly IQuizzRepository _quizzRepository;

        public UnitOfWork(AppDBContext appDBContext,
            IClassRepository classRepository,
            IQuizzRepository quizzRepository)
        {
            _appDBContext = appDBContext;
            _classRepository = classRepository;
            _quizzRepository = quizzRepository;
        }

        public IClassRepository ClassRepository => _classRepository;
        public IQuizzRepository QuizzRepository => _quizzRepository;

        public async Task<int> SaveChangeAsync() => await _appDBContext.SaveChangesAsync();
    }
}
