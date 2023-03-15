using Applications.ViewModels.Response;

namespace Applications.Interfaces
{
    public interface IClassTrainingProgramService
    {
        Task<Response> GetAllClassTrainingProgram(int pageIndex = 0, int pageSize = 10);
    }
}
