using Domain.EntityRelationship;

namespace Applications.Repositories
{
    public interface IClassTrainingProgramRepository : IGenericRepository<ClassTrainingProgram>
    {
        Task<ClassTrainingProgram> GetClassTrainingProgram(Guid ClassId, Guid TrainingProgramId);
    }
}
