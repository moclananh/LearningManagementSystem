using Application.ViewModels.TrainingProgramModels;
using Applications.Commons;
using Applications.ViewModels.TrainingProgramModels;
using Applications.ViewModels.TrainingProgramSyllabi;

namespace Applications.Interfaces
{
    public interface ITrainingProgramService
    {
        Task<Pagination<TrainingProgramViewModel>> ViewAllTrainingProgramAsync(int pageIndex = 0, int pageSize = 10);
        Task<Pagination<TrainingProgramViewModel>> GetTrainingProgramByClassId(Guid ClassId, int pageIndex = 0, int pageSize = 10);
        Task<TrainingProgramViewModel> GetTrainingProgramById(Guid TrainingProramId);
        Task<Pagination<TrainingProgramViewModel>> ViewTrainingProgramEnableAsync(int pageIndex = 0, int pageSize = 10);
        Task<Pagination<TrainingProgramViewModel>> ViewTrainingProgramDisableAsync(int pageIndex = 0, int pageSize = 10);
        Task<CreateTrainingProgramViewModel?> CreateTrainingProgramAsync(CreateTrainingProgramViewModel TrainingProgramDTO);
        Task<UpdateTrainingProgramViewModel?> UpdateTrainingProgramAsync(Guid TrainingProgramId, UpdateTrainingProgramViewModel TrainingProgramDTO);
        Task<CreateTrainingProgramSyllabi> AddSyllabusToTrainingProgram(Guid SyllabusId, Guid TrainingProgramId);
        Task<CreateTrainingProgramSyllabi> RemoveSyllabusToTrainingProgram(Guid SyllabusId, Guid TrainingProgramId);
    }
}
