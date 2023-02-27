using Application.ViewModels.UnitViewModels;
using Applications.Commons;

namespace Applications.Repositories
{
    public interface IUnitServices
    {
        public Task<Pagination<UnitViewModel>> GetAllUnits(int pageIndex = 0, int pageSize = 10);
        public Task<CreateUnitViewModel> CreateUnitAsync(CreateUnitViewModel UnitDTO);
        public Task<CreateUnitViewModel> UpdateUnitAsync(Guid UnitId, CreateUnitViewModel UnitDTO);
        public Task<UnitViewModel> ViewUnitById(Guid UnitId);
        public Task<Pagination<UnitViewModel>> ViewEnableUnitsAsync(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<UnitViewModel>> ViewDisableUnitsAsync(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<CreateUnitViewModel>> GetUnitByModuleIdAsync(Guid ModuleId, int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<UnitViewModel>> GetUnitByNameAsync(string UnitName, int pageIndex = 0, int pageSize = 10);
    }
}
