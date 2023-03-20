using Applications.Commons;
using Applications.ViewModels.Response;
using Applications.ViewModels.UnitModuleViewModel;


namespace Applications.Interfaces
{
   public interface IModuleUnitService
   {
        public Task<Response> GetAllModuleUnitsAsync(int pageIndex = 0, int pageSize = 10);
    }
}
