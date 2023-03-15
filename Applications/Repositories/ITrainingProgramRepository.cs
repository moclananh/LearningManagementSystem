using Applications.Commons;
using Domain.Entities;

namespace Applications.Repositories
{
    public interface ITrainingProgramRepository : IGenericRepository<TrainingProgram>
    {
        Task<Pagination<TrainingProgram>> GetTrainingProgramByClassId(Guid ClassId, int pageNumber = 0, int pageSize = 10);
        Task<Pagination<TrainingProgram>> GetTrainingProgramEnable(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<TrainingProgram>> GetTrainingProgramDisable(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<TrainingProgram>> GetTrainingProgramByName(string name, int pageNumber =0, int pageSize = 10);
        Task<TrainingProgram> GetTrainingProgramDetails(Guid TrainingProgramId);
    }
}
