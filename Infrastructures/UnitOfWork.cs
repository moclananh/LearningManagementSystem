using Applications;
using Applications.IRepositories;
using Applications.Repositories;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _appDBContext;
        private readonly IClassRepository _classRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IQuizzRepository _quizzRepository;
        private readonly IUserRepository _userRepository;
        public UnitOfWork(AppDBContext appDBContext,
            IClassRepository classRepository, 
            IAssignmentRepository assignmentRepository, 
            IQuizzRepository quizzRepository, 
            IUserRepository userRepository)
        {
            _appDBContext = appDBContext;
            _classRepository = classRepository;
            _assignmentRepository = assignmentRepository;
            _quizzRepository = quizzRepository;
            _userRepository = userRepository;
        }

        public IClassRepository ClassRepository => _classRepository;
        public IAssignmentRepository AssignmentRepository => _assignmentRepository;
        public IQuizzRepository QuizzRepository => _quizzRepository;
        public IUserRepository UserRepository => _userRepository;
        public async Task<int> SaveChangeAsync() => await _appDBContext.SaveChangesAsync();
    }
}
