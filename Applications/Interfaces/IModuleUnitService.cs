using Applications.Commons;
using Applications.ViewModels.UnitModuleViewModel;


namespace Applications.Interfaces
{
   public interface IModuleUnitService
   {
        public Task<Pagination<ModuleUnitViewModel>> GetAllModuleUnitsAsync(int pageIndex = 0, int pageSize = 10);
    }
}
