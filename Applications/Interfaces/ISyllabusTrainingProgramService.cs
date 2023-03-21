using Applications.ViewModels.Response;

namespace Applications.Interfaces
{
    public interface ISyllabusTrainingProgramService
    {
        public Task<Response> GetAllSyllabusTrainingPrograms(int pageIndex = 0, int pageSize = 10);
        public Task<Response> AddMultipleSyllabusesToTrainingProgram(Guid trainingProgramId, List<Guid> syllabusesId);
    }
}
