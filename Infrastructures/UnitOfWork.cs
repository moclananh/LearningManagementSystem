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
        private readonly IUnitRepository _unitRepository;
        private readonly IModuleRepository _moduleRepository;
        public UnitOfWork(AppDBContext appDBContext,
            IClassRepository classRepository, 
            IAssignmentRepository assignmentRepository, 
            IQuizzRepository quizzRepository, 
            IUserRepository userRepository,
            IUnitRepository unitRepository,          
            IModuleRepository moduleRepository)
        {
            _appDBContext = appDBContext;
            _classRepository = classRepository;
            _assignmentRepository = assignmentRepository;
            _quizzRepository = quizzRepository;
            _userRepository = userRepository;
            _unitRepository = unitRepository;
            _moduleRepository = moduleRepository;
        }

        public IClassRepository ClassRepository => _classRepository;
        public IAssignmentRepository AssignmentRepository => _assignmentRepository;
        public IQuizzRepository QuizzRepository => _quizzRepository;
        public IUserRepository UserRepository => _userRepository;
        public IUnitRepository UnitRepository => _unitRepository;
        public IModuleRepository ModuleRepository => _moduleRepository;
        public async Task<int> SaveChangeAsync() => await _appDBContext.SaveChangesAsync();
    }
}
