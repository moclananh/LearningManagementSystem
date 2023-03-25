using Applications.Commons;
using Applications.ViewModels.ClassTrainingProgramViewModels;
using Applications.ViewModels.ClassUserViewModels;
using Applications.ViewModels.ClassViewModels;
using Applications.ViewModels.Response;
using Domain.Entities;

namespace Applications.Interfaces
{
    public interface IClassService
    {
        public Task<Pagination<ClassViewModel>> GetAllClasses(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<ClassViewModel>> GetEnableClasses(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<ClassViewModel>> GetDisableClasses(int pageIndex = 0, int pageSize = 10);
        public Task<ClassViewModel> GetClassById(Guid ClassId);
        public Task<Pagination<ClassViewModel>> GetClassByName(string Name, int pageIndex = 0, int pageSize = 10);
        public Task<ClassViewModel?> CreateClass(CreateClassViewModel classDTO);
        public Task<UpdateClassViewModel?> UpdateClass(Guid ClassId, UpdateClassViewModel classDTO);
        public Task<CreateClassTrainingProgramViewModel> AddTrainingProgramToClass(Guid ClassId, Guid TrainingProgramId);
        public Task<CreateClassTrainingProgramViewModel> RemoveTrainingProgramFromClass(Guid ClassId, Guid TrainingProgramId);
        public Task<CreateClassUserViewModel> RemoveUserFromClass(Guid ClassId, Guid UserId);
        public Task<Pagination<ClassViewModel>> GetClassByFilter(ClassFiltersViewModel filters, int pageNumber = 0, int pageSize = 10);
        public Task<ClassDetailsViewModel> GetClassDetails(Guid ClassId);
        public Task<Class?> GetClassByClassCode(string classCode);
        public Task<ClassViewModel> ApprovedClass(Guid ClassId);
        public Task<ClassViewModel> AddUserToClass(Guid ClassId, Guid UserId);
        public Task<Response> UpdateStatusOnlyOfClass(Guid ClassId, UpdateStatusOnlyOfClass ClassDTO);
    }
}
