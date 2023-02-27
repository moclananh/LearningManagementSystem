using Application.ViewModels.UnitViewModels;
using Applications.Commons;
using Applications.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IUnitServices _unitServices;
        public UnitController(IUnitServices unitServices)
        {
            _unitServices = unitServices;
        }

        [HttpGet("GetAllUnit")]
        public async Task<Pagination<UnitViewModel>> GetUnitsPagingsion(int pageIndex = 0, int pageSize = 10) => await _unitServices.GetAllUnits(pageIndex, pageSize);

        [HttpPost("CreateUnit")]
        public async Task<CreateUnitViewModel> CreateUnit(CreateUnitViewModel UnitModel) => await _unitServices.CreateUnitAsync(UnitModel); 

        [HttpGet("ViewUnitById/{UnitId}")]
        public async Task<UnitViewModel> GetUnitById(Guid UnitId) => await _unitServices.ViewUnitById(UnitId);

        [HttpPut("UpdateUnit/{UnitId}")]
        public async Task<CreateUnitViewModel> UpdateUnit(Guid UnitId, CreateUnitViewModel UnitModel) => await _unitServices.UpdateUnitAsync(UnitId, UnitModel);

        [HttpGet("GetEnableUnits")]
        public async Task<Pagination<UnitViewModel>> GetEnableUnits(int pageIndex = 0, int pageSize = 10) => await _unitServices.ViewEnableUnitsAsync(pageIndex, pageSize);
        [HttpGet("GetDisableUnits")]
        public async Task<Pagination<UnitViewModel>> GetDiableClasses(int pageIndex = 0, int pageSize = 10) => await _unitServices.ViewDisableUnitsAsync(pageIndex, pageSize);
               
        [HttpGet("GetUnitsByModuleId/{ModuleId}")]
        public async Task<Pagination<CreateUnitViewModel>> GetUnitByModuleIdAsync(Guid ModuleId, int pageIndex = 0, int pageSize = 10)
        {
            return await _unitServices.GetUnitByModuleIdAsync(ModuleId, pageIndex, pageSize);
        }
        [HttpGet("GetUnitsByName/{UnitName}")]
        public async Task<Pagination<UnitViewModel>> GetUnitByNameAsync(string UnitName, int pageIndex = 0, int pageSize = 10)
        {
            return await _unitServices.GetUnitByNameAsync(UnitName, pageIndex, pageSize);
        }
    }
}

