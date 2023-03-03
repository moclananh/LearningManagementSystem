using Applications.Commons;
using Applications.ViewModels.ModuleViewModels;
using Applications.ViewModels.Response;
using Applications.ViewModels.UnitModuleViewModel;

namespace Applications.Interfaces
{
    public interface IModuleService
    {
        public Task<Response> GetAllModules(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetEnableModules(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetDisableModules(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetModulesBySyllabusId(Guid syllabusId, int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetModuleById(Guid moduleId);
        public Task<Response> GetModulesByName(string name, int pageIndex = 0, int pageSize = 10);
        public Task<CreateModuleViewModel?> CreateModule(CreateModuleViewModel moduleDTO);
        public Task<UpdateModuleViewModel> UpdateModule(Guid moduleId, UpdateModuleViewModel moduleDTO);
        public Task<ModuleUnitViewModel> AddUnitToModule(Guid ModuleId, Guid UnitId);
        public Task<ModuleUnitViewModel> RemoveUnitToModule(Guid ModuleId, Guid UnitId);
    }
}
