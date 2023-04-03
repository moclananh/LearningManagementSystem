using Application.ViewModels.UnitViewModels;
using Applications.Commons;
using Applications.ViewModels.Response;

namespace Applications.Interfaces
{
    public interface IUnitServices
    {
        public Task<Response> CreateUnitAsync(CreateUnitViewModel UnitDTO);
        public Task<UnitViewModel> UpdateUnitAsync(Guid UnitId, CreateUnitViewModel UnitDTO);
        public Task<Response> GetUnitById(Guid UnitId);
        public Task<Response> GetAllUnits(int pageNumber = 0, int pageSize = 10);
        public Task<Response> GetEnableUnitsAsync(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetDisableUnitsAsync(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetUnitByModuleIdAsync(Guid ModuleId, int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetUnitByNameAsync(string UnitName, int pageIndex = 0, int pageSize = 10);
    }
}
