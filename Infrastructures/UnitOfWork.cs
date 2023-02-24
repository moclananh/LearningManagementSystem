using Application.Repositories;
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
        private readonly IClassUserRepository _classUserRepository;
        private readonly IAuditPlanRepository _auditPlanRepository;
        private readonly ILectureRepository _lectureRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly ITrainingProgramRepository _trainingProgramRepository;
        private readonly IOutputStandardRepository _outputStandardRepository;
        private readonly ISyllabusRepository _syllabusRepository;
        private readonly IAssignmentQuestionRepository _assignmentquestionRepository;
        private readonly IClassTrainingProgramRepository _classTrainingProgramRepository;

        public UnitOfWork(AppDBContext appDBContext,
            IClassRepository classRepository,
            IAssignmentRepository assignmentRepository,
            IQuizzRepository quizzRepository,
            IUserRepository userRepository,
            IClassUserRepository classUserRepository,
            ILectureRepository lectureRepository,
            IUnitRepository unitRepository,
            IModuleRepository moduleRepository,
            IAuditPlanRepository auditPlanRepository,
            ITrainingProgramRepository trainingProgramRepository,
            IOutputStandardRepository outputStandardRepository,
            ISyllabusRepository syllabusRepository,
            IAssignmentQuestionRepository assignmentQuestionRepository,
            IClassTrainingProgramRepository classTrainingProgramRepository)

        {
            _appDBContext = appDBContext;
            _classRepository = classRepository;
            _assignmentRepository = assignmentRepository;
            _quizzRepository = quizzRepository;
            _userRepository = userRepository;
            _classUserRepository = classUserRepository;
            _auditPlanRepository = auditPlanRepository;
            _lectureRepository = lectureRepository;
            _unitRepository = unitRepository;
            _moduleRepository = moduleRepository;
            _trainingProgramRepository = trainingProgramRepository;
            _outputStandardRepository = outputStandardRepository;
            _syllabusRepository = syllabusRepository;
            _assignmentquestionRepository = assignmentQuestionRepository;
            _classTrainingProgramRepository = classTrainingProgramRepository;
        }

        public IClassRepository ClassRepository => _classRepository;
        public IAssignmentRepository AssignmentRepository => _assignmentRepository;
        public IQuizzRepository QuizzRepository => _quizzRepository;
        public IUserRepository UserRepository => _userRepository;
        public IClassUserRepository ClassUserRepository => _classUserRepository;
        public IAuditPlanRepository AuditPlanRepository => _auditPlanRepository;
        public ILectureRepository LectureRepository => _lectureRepository;
        public IUnitRepository UnitRepository => _unitRepository;
        public IModuleRepository ModuleRepository => _moduleRepository;
        public ITrainingProgramRepository TrainingProgramRepository => _trainingProgramRepository;
        public IOutputStandardRepository OutputStandardRepository => _outputStandardRepository;
        public ISyllabusRepository SyllabusRepository => _syllabusRepository;
        public IAssignmentQuestionRepository AssignmentQuestionRepository => _assignmentquestionRepository;
        public IClassTrainingProgramRepository ClassTrainingProgramRepository => _classTrainingProgramRepository;

        public async Task<int> SaveChangeAsync() => await _appDBContext.SaveChangesAsync();
    }
}
