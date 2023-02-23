using Application.ViewModels.UnitViewModels;

namespace Applications.Interfaces
{
    public interface IUnitServices
    {
        public Task<List<UnitViewModel>> ViewAllUnitAsync();
        public Task<CreateUnitViewModel> CreateUnitAsync(CreateUnitViewModel UnitDTO);
        public Task<CreateUnitViewModel> UpdateUnitAsync(Guid UnitId, CreateUnitViewModel UnitDTO);
        public Task<UnitViewModel> ViewUnitById(Guid UnitId);
        public Task<List<UnitViewModel>> ViewEnableUnitsAsync();
        public Task<List<UnitViewModel>> ViewDisableUnitsAsync();
    }
}
