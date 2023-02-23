using Application.ViewModels.TrainingProgramModels;

namespace Applications.Interfaces
{
    public interface ITrainingProgramService
    {
        Task<List<ViewTrainingProgram>> ViewAllTrainingProgramAsync();
        Task<List<ViewTrainingProgram>> GetTrainingProgramByClassId(Guid ClassId);
        Task<ViewTrainingProgram> GetTrainingProgramById(Guid TrainingProramId);
        Task<List<ViewTrainingProgram>> ViewTrainingProgramEnableAsync();
        Task<List<ViewTrainingProgram>> ViewTrainingProgramDisableAsync();
        Task<ViewTrainingProgram?> CreateTrainingProgramAsync(ViewTrainingProgram TrainingProgramDTO);
        Task<ViewTrainingProgram?> UpdateTrainingProgramAsync(Guid TrainingProgramId, ViewTrainingProgram TrainingProgramDTO);
    }
}
