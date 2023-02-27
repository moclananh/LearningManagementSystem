using Application.ViewModels.TrainingProgramModels;
using Applications.Commons;
using Applications.ViewModels.TrainingProgramSyllabi;

namespace Applications.Interfaces
{
    public interface ITrainingProgramService
    {
        Task<Pagination<ViewTrainingProgram>> ViewAllTrainingProgramAsync(int pageIndex = 0, int pageSize = 10);
        Task<Pagination<ViewTrainingProgram>> GetTrainingProgramByClassId(Guid ClassId, int pageIndex = 0, int pageSize = 10);
        Task<ViewTrainingProgram> GetTrainingProgramById(Guid TrainingProramId);
        Task<Pagination<ViewTrainingProgram>> ViewTrainingProgramEnableAsync(int pageIndex = 0, int pageSize = 10);
        Task<Pagination<ViewTrainingProgram>> ViewTrainingProgramDisableAsync(int pageIndex = 0, int pageSize = 10);
        Task<ViewTrainingProgram?> CreateTrainingProgramAsync(ViewTrainingProgram TrainingProgramDTO);
        Task<ViewTrainingProgram?> UpdateTrainingProgramAsync(Guid TrainingProgramId, ViewTrainingProgram TrainingProgramDTO);
        Task<CreateTrainingProgramSyllabi> AddSyllabusToTrainingProgram(Guid SyllabusId, Guid TrainingProgramId) ;
        Task<CreateTrainingProgramSyllabi> RemoveSyllabusToTrainingProgram(Guid SyllabusId, Guid TrainingProgramId);
    }
}
