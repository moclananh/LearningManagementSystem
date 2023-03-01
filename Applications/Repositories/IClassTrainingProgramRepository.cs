using Applications.Commons;
using Domain.EntityRelationship;

namespace Applications.Repositories
{
    public interface IClassTrainingProgramRepository : IGenericRepository<ClassTrainingProgram>
    {
        Task<ClassTrainingProgram> GetClassTrainingProgram(Guid ClassId, Guid TrainingProgramId);
        Task<Pagination<ClassTrainingProgram>> GetAllClassTrainingProgram(int pageNumber = 0, int pageSize = 10);
    }
}
