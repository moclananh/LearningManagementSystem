using Domain.Entities;

namespace Applications.Repositories
{
    public interface ITrainingProgramRepository:IGenericRepository<TrainingProgram>
    {
        Task<List<TrainingProgram>> GetTrainingProgramByClassId(Guid ClassId);
        Task<List<TrainingProgram>> GetTrainingProgramEnable();
        Task<List<TrainingProgram>> GetTrainingProgramDisable();
    }
}
