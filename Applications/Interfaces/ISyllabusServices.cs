using Applications.Commons;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusModuleViewModel;
using Applications.ViewModels.SyllabusViewModels;
using Applications.ViewModels.UnitModuleViewModel;

namespace Applications.Interfaces
{
    public interface ISyllabusServices
    {
        public Task<CreateSyllabusViewModel?> CreateSyllabus(CreateSyllabusViewModel SyllabusDTO);
        public Task<UpdateSyllabusViewModel?> UpdateSyllabus(Guid SyllabusId, UpdateSyllabusViewModel SyllabusDTO);
        public Task<Pagination<SyllabusViewModel>> GetAllSyllabus(int pageNumber = 0, int pageSize = 10);
        public Task<Pagination<SyllabusViewModel>> GetEnableSyllabus(int pageNumber = 0, int pageSize = 10);
        public Task<Pagination<SyllabusViewModel>> GetDisableSyllabus(int pageNumber = 0, int pageSize = 10);
        public Task<Response> GetSyllabusById(Guid SyllabusId);
        public Task<Pagination<SyllabusViewModel>> GetSyllabusByName(string SyllabusName, int pageNumber = 0, int pageSize = 10);
        public Task<Pagination<SyllabusViewModel>> GetSyllabusByTrainingProgramId(Guid TrainingProgramId, int pageNumber = 0, int pageSize = 10);
        public Task<Response> GetSyllabusByOutputStandardId(Guid OutputStandardId, int pageNumber = 0, int pageSize = 10);
        public Task<SyllabusModuleViewModel> AddSyllabusModule(Guid SyllabusId, Guid ModuleId);
        public Task<SyllabusModuleViewModel> RemoveSyllabusModule(Guid SyllabusId, Guid ModuleId);

    }
}
