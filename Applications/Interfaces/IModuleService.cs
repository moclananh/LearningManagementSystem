using Applications.Commons;
using Applications.ViewModels.ModuleViewModels;
using Applications.ViewModels.UnitModuleViewModel;

namespace Applications.Interfaces
{
    public interface IModuleService
    {
        public Task<Pagination<ModuleViewModels>> GetAllModules(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<ModuleViewModels>> GetEnableModules(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<ModuleViewModels>> GetDisableModules(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<ModuleViewModels>> GetModulesBySyllabusId(Guid syllabusId, int pageIndex = 0, int pageSize = 10);
        public Task<ModuleViewModels> GetModuleById(Guid moduleId);
        public Task<Pagination<ModuleViewModels>> GetModulesByName(string name, int pageIndex = 0, int pageSize = 10);
        public Task<CreateModuleViewModel?> CreateModule(CreateModuleViewModel moduleDTO);
        public Task<UpdateModuleViewModel> UpdateModule(Guid moduleId, UpdateModuleViewModel moduleDTO);
        public Task<ModuleUnitViewModel> AddUnitToModule(Guid ModuleId, Guid UnitId);
        public Task<ModuleUnitViewModel> RemoveUnitToModule(Guid ModuleId, Guid UnitId);
    }
}
