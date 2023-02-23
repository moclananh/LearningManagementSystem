using Applications;
using Applications.IRepositories;
using Applications.Repositories;
using Infrastructures.Repositories;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _appDBContext;
        private readonly IClassRepository _classRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IQuizzRepository _quizzRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuditPlanRepository _auditPlanRepository;
        private readonly ILectureRepository _lectureRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IModuleRepository _moduleRepository;
        public UnitOfWork(AppDBContext appDBContext,
            IClassRepository classRepository, 
            IAssignmentRepository assignmentRepository, 
            IQuizzRepository quizzRepository, 
            IUserRepository userRepository,
            ILectureRepository lectureRepository,
            IUnitRepository unitRepository,          
            IModuleRepository moduleRepository,
            IAuditPlanRepository auditPlanRepository)

        {
            _appDBContext = appDBContext;
            _classRepository = classRepository;
            _assignmentRepository = assignmentRepository;
            _quizzRepository = quizzRepository;
            _userRepository = userRepository;
            _auditPlanRepository = auditPlanRepository;
            _lectureRepository = lectureRepository;
            _unitRepository = unitRepository;
            _moduleRepository = moduleRepository;
        }

        public IClassRepository ClassRepository => _classRepository;
        public IAssignmentRepository AssignmentRepository => _assignmentRepository;
        public IQuizzRepository QuizzRepository => _quizzRepository;
        public IUserRepository UserRepository => _userRepository;
        public IAuditPlanRepository AuditPlanRepository => _auditPlanRepository;
        public ILectureRepository LectureRepository => _lectureRepository;
        public IUnitRepository UnitRepository => _unitRepository;
        public IModuleRepository ModuleRepository => _moduleRepository;
        public async Task<int> SaveChangeAsync() => await _appDBContext.SaveChangesAsync();
    }
}
