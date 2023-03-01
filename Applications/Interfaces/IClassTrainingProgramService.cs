using Applications.Commons;
using Applications.ViewModels.ClassTrainingProgramViewModels;

namespace Applications.Interfaces
{
    public interface IClassTrainingProgramService
    {
        Task<Pagination<ClassTrainingProgramViewModel>> GetAllClassTrainingProgram(int pageIndex = 0, int pageSize = 10);
    }
}
