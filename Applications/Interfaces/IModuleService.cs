using Applications.ViewModels.ModuleViewModels;
using Applications.ViewModels.UnitModuleViewModel;

namespace Applications.Interfaces
{
    public interface IModuleService
    {
        public Task<List<ModuleViewModels>> GetAllModules();
        public Task<List<ModuleViewModels>> GetEnableModules();
        public Task<List<ModuleViewModels>> GetDisableModules();
        public Task<List<ModuleViewModels>> GetModulesBySyllabusId(Guid syllabusId);
        public Task<ModuleViewModels> GetModuleById(Guid moduleId);
        public Task<List<ModuleViewModels>> GetModulesByName(string name);
        public Task<ModuleViewModels?> CreateModule(ModuleViewModels moduleDTO);
        public Task<ModuleViewModels> UpdateModule(Guid moduleId, ModuleViewModels moduleDTO);

        public Task<ModuleUnitViewModel> AddUnitToModule(Guid ModuleId, Guid UnitId);
    }
}
