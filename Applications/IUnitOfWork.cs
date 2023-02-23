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
        public ILectureRepository LectureRepository { get; }
        public IUnitRepository UnitRepository { get; }
        public IModuleRepository ModuleRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
