 using Applications.Commons;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusModuleViewModel;


namespace Applications.Interfaces
{
    public interface ISyllabusModuleService
    {
        public Task<Pagination<SyllabusModuleViewModel>> GetAllSyllabusModuleAsync(int pageIndex = 0, int pageSize = 10);
        public Task<Response> AddMultiModulesToSyllabus(Guid syllabusId, List<Guid> moduleIds);
    }
}
