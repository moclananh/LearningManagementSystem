using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusModuleViewModel;
using Applications.ViewModels.SyllabusViewModels;

namespace Applications.Interfaces
{
    public interface ISyllabusServices
    {
        public Task<SyllabusViewModel?> CreateSyllabus(CreateSyllabusViewModel SyllabusDTO);
        public Task<Response> CreateSyllabusDetail(CreateSyllabusDetailModel SyllabusDTO);
        public Task<UpdateSyllabusViewModel?> UpdateSyllabus(Guid SyllabusId, UpdateSyllabusViewModel SyllabusDTO);
        public Task<Response> GetAllSyllabus(int pageNumber = 0, int pageSize = 10);
        public Task<Response> GetEnableSyllabus(int pageNumber = 0, int pageSize = 10);
        public Task<Response> GetDisableSyllabus(int pageNumber = 0, int pageSize = 10);
        public Task<Response> GetSyllabusById(Guid SyllabusId);
        public Task<Response> GetSyllabusByName(string SyllabusName, int pageNumber = 0, int pageSize = 10);
        public Task<Response> GetSyllabusByTrainingProgramId(Guid TrainingProgramId, int pageNumber = 0, int pageSize = 10);
        public Task<Response> GetSyllabusByOutputStandardId(Guid OutputStandardId, int pageNumber = 0, int pageSize = 10);
        public Task<SyllabusModuleViewModel> AddSyllabusModule(Guid SyllabusId, Guid ModuleId);
        public Task<SyllabusModuleViewModel> RemoveSyllabusModule(Guid SyllabusId, Guid ModuleId);
        public Task<Response> GetSyllabusDetails(Guid syllabusId);
        public Task<Response> GetAllSyllabusDetail(int pageNumber = 0, int pageSize = 10);
        public Task<Response> GetSyllabusByCreationDate(DateTime startDate, DateTime endDate, int pageNumber = 0, int pageSize = 10);
        public Task<Response> UpdateStatusOnlyOfSyllabus(Guid SyllabusId, UpdateStatusOnlyOfSyllabus SyllabusDTO);
    }
}
