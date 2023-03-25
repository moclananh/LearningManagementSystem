using Application.ViewModels.TrainingProgramModels;
using Applications.Commons;
using Applications.ViewModels.Response;
using Applications.ViewModels.TrainingProgramModels;
using Applications.ViewModels.TrainingProgramSyllabi;

namespace Applications.Interfaces
{
    public interface ITrainingProgramService
    {
        Task<Response> ViewAllTrainingProgramAsync(int pageIndex = 0, int pageSize = 10);
        Task<Response> GetTrainingProgramByClassId(Guid ClassId, int pageIndex = 0, int pageSize = 10);
        Task<Response> GetTrainingProgramById(Guid TrainingProramId);
        Task<Response> ViewTrainingProgramEnableAsync(int pageIndex = 0, int pageSize = 10);
        Task<Response> ViewTrainingProgramDisableAsync(int pageIndex = 0, int pageSize = 10);
        Task<TrainingProgramViewModel?> CreateTrainingProgramAsync(CreateTrainingProgramViewModel TrainingProgramDTO);
        Task<UpdateTrainingProgramViewModel?> UpdateTrainingProgramAsync(Guid TrainingProgramId, UpdateTrainingProgramViewModel TrainingProgramDTO);
        Task<CreateTrainingProgramSyllabi> AddSyllabusToTrainingProgram(Guid SyllabusId, Guid TrainingProgramId);
        Task<CreateTrainingProgramSyllabi> RemoveSyllabusToTrainingProgram(Guid SyllabusId, Guid TrainingProgramId);
        Task<Response> GetByName(string name, int pageIndex = 0, int pageSize = 10);
        Task<Response> GetTrainingProgramDetails(Guid TrainingProgramId);
        Task<Response> UpdateStatusOnlyOfTrainingProgram(Guid TrainningProgramId, UpdateStatusOnlyOfTrainingProgram trainingProgramDTO);
    }
}
