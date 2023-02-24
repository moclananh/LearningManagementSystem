using Application.ViewModels.UnitViewModels;
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

        [HttpGet("GetAll")]
        public async Task<List<UnitViewModel>> ViewAllUnitAsync() => await _unitServices.ViewAllUnitAsync();

        [HttpPost("CreateUnit")]
        public async Task<CreateUnitViewModel> CreateUnit(CreateUnitViewModel UnitModel) => await _unitServices.CreateUnitAsync(UnitModel); 

        [HttpGet("ViewUnitById/{UnitId}")]
        public async Task<UnitViewModel> GetUnitById(Guid UnitId) => await _unitServices.ViewUnitById(UnitId);

        [HttpPut("UpdateUnit/{UnitId}")]
        public async Task<CreateUnitViewModel> UpdateUnit(Guid UnitId, CreateUnitViewModel UnitModel) => await _unitServices.UpdateUnitAsync(UnitId, UnitModel);

        [HttpGet("GetEnableUnits")]
        public async Task<List<UnitViewModel>> GetEnableUnits() => await _unitServices.ViewEnableUnitsAsync();

        [HttpGet("GetDisableUnits")]
        public async Task<List<UnitViewModel>> GetDiableClasses() => await _unitServices.ViewDisableUnitsAsync();
    }
}
