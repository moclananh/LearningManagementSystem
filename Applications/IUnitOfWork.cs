using Application.Repositories;
using Applications.Interfaces;
using Applications.IRepositories;
using Applications.Repositories;

namespace Applications
{
    public interface IUnitOfWork
    {
        public IClassRepository ClassRepository { get; }
        public IAssignmentRepository AssignmentRepository { get; }
        public IQuizzRepository QuizzRepository { get; }
        public IUserRepository UserRepository { get; }
        public IClassUserRepository ClassUserRepository { get; }
        public IAuditPlanRepository AuditPlanRepository { get; }
        public ILectureRepository LectureRepository { get; }
        public IUnitRepository UnitRepository { get; }
        public IModuleRepository ModuleRepository { get; }
        public ITrainingProgramRepository TrainingProgramRepository { get; }
        public IOutputStandardRepository OutputStandardRepository { get; }
        public ISyllabusRepository SyllabusRepository { get; }
        public IAssignmentQuestionRepository AssignmentQuestionRepository { get; }
        public IClassTrainingProgramRepository ClassTrainingProgramRepository { get; }
        public IPracticeRepository PracticeRepository { get; }
        public IAuditResultRepository AuditResultRepository { get; }
        public IPracticeQuestionRepository PracticeQuestionRepository { get; }
        public ITrainingProgramSyllabiRepository TrainingProgramSyllabiRepository { get; }
        public ISyllabusOutputStandardRepository SyllabusOutputStandardRepository { get; }
        public IModuleUnitRepository ModuleUnitRepository { get; }
        public IUserAuditPlanRepository UserAuditPlanRepository { get; }
        public IQuizzQuestionRepository QuizzQuestionRepository { get; }
        public ISyllabusModuleRepository SyllabusModuleRepository { get; }
        public IAttendanceRepository AttendanceRepository { get; }
        public IAbsentRequestRepository AbsentRequestRepository { get; }
        public IRefreshTokenRepository RefreshTokenRepository { get; }

        public Task<int> SaveChangeAsync();
    }
}
