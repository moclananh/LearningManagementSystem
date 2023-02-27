using Domain.EntityRelationship;

namespace Applications.Repositories
{
    public interface ITrainingProgramSyllabiRepository : IGenericRepository<TrainingProgramSyllabus>
    {
        Task<TrainingProgramSyllabus> GetTrainingProgramSyllabus(Guid SyllabusId, Guid TrainingProgramId);
    }
}
