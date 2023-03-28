using Application.Repositories;
using Applications;
using Applications.Interfaces;
using Applications.IRepositories;
using Applications.Repositories;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _appDBContext;
        private readonly IClassRepository _classRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAttendanceRepository _attendanceRepository;
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
        private readonly IPracticeRepository _practiceRepository;
        private readonly IAuditResultRepository _auditResultRepository;
        private readonly IPracticeQuestionRepository _practicequestionRepository;
        private readonly ITrainingProgramSyllabiRepository _trainingProgramSyllabiRepository;
        private readonly ISyllabusOutputStandardRepository _syllabusOutputStandardRepository;
        private readonly IModuleUnitRepository _moduleUnitRepository;
        private readonly IUserAuditPlanRepository _userAuditPlanRepository;
        private readonly IQuizzQuestionRepository _quizzQuestionRepository;
        private readonly ISyllabusModuleRepository _syllabusModuleRepository;
        private readonly IAbsentRequestRepository _absentRequestRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

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
            IClassTrainingProgramRepository classTrainingProgramRepository,
            IPracticeQuestionRepository practicequestionRepository,
            ITrainingProgramSyllabiRepository trainingProgramSyllabiRepository,
            ISyllabusOutputStandardRepository syllabusOutputStandardRepository,
            IPracticeRepository practiceRepository,
            IAuditResultRepository auditResultRepository,
            IModuleUnitRepository moduleUnitRepository,
            IUserAuditPlanRepository userAuditPlanRepository,
            IQuizzQuestionRepository quizzQuestionRepository,
            ISyllabusModuleRepository syllabusModuleRepository,
            IAttendanceRepository attendanceRepository,
            IAbsentRequestRepository absentRequestRepository,
            IRefreshTokenRepository RefreshTokenRepository)

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
            _practiceRepository = practiceRepository;
            _auditResultRepository = auditResultRepository;
            _practicequestionRepository = practicequestionRepository;
            _trainingProgramSyllabiRepository = trainingProgramSyllabiRepository;
            _syllabusOutputStandardRepository = syllabusOutputStandardRepository;
            _moduleUnitRepository = moduleUnitRepository;
            _userAuditPlanRepository = userAuditPlanRepository;
            _quizzQuestionRepository = quizzQuestionRepository;
            _syllabusModuleRepository = syllabusModuleRepository;
            _attendanceRepository = attendanceRepository;
            _absentRequestRepository = absentRequestRepository;
            _refreshTokenRepository = RefreshTokenRepository;
        }
        public IClassRepository ClassRepository => _classRepository;
        public IPracticeRepository PracticeRepository => _practiceRepository;
        public IAuditResultRepository AuditResultRepository => _auditResultRepository;
        public IAssignmentRepository AssignmentRepository => _assignmentRepository;
        public IAttendanceRepository AttendanceRepository => _attendanceRepository;
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
        public IPracticeQuestionRepository PracticeQuestionRepository => _practicequestionRepository;
        public ITrainingProgramSyllabiRepository TrainingProgramSyllabiRepository => _trainingProgramSyllabiRepository;
        public ISyllabusOutputStandardRepository SyllabusOutputStandardRepository => _syllabusOutputStandardRepository;
        public IModuleUnitRepository ModuleUnitRepository => _moduleUnitRepository;
        public IUserAuditPlanRepository UserAuditPlanRepository => _userAuditPlanRepository;
        public IQuizzQuestionRepository QuizzQuestionRepository => _quizzQuestionRepository;
        public ISyllabusModuleRepository SyllabusModuleRepository => _syllabusModuleRepository;
        public IAbsentRequestRepository AbsentRequestRepository => _absentRequestRepository;
        public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository;
        public async Task<int> SaveChangeAsync() => await _appDBContext.SaveChangesAsync();
    }
}
