using Applications.Commons;
using Applications.ViewModels.ClassTrainingProgramViewModels;
using Applications.ViewModels.ClassViewModels;

namespace Applications.Interfaces
{
    public interface IClassServices
    {
        public Task<Pagination<ClassViewModel>> GetAllClasses(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<ClassViewModel>> GetEnableClasses(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<ClassViewModel>> GetDisableClasses(int pageIndex = 0, int pageSize = 10);
        public Task<ClassViewModel> GetClassById(Guid ClassId);
        public Task<Pagination<ClassViewModel>> GetClassByName(string Name, int pageIndex = 0, int pageSize = 10);
        public Task<CreateClassViewModel?> CreateClass(CreateClassViewModel classDTO);
        public Task<UpdateClassViewModel?> UpdateClass(Guid ClassId, UpdateClassViewModel classDTO);
        public Task<CreateClassTrainingProgramViewModel> AddTrainingProgramToClass(Guid ClassId, Guid TrainingProgramId);
        public Task<CreateClassTrainingProgramViewModel> RemoveTrainingProgramToClass(Guid ClassId, Guid TrainingProgramId);
    }
}
