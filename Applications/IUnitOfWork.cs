using Application.Repositories;
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
        public ISyllabusRepository SyllabusRepository { get; }
        public IAssignmentQuestionRepository AssignmentQuestionRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
