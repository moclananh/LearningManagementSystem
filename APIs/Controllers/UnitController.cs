using Application.ViewModels.UnitViewModels;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using FluentValidation;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IUnitServices _unitServices;
        private readonly IValidator<CreateUnitViewModel> _unitValidation;

        public UnitController(IUnitServices unitServices, IValidator<CreateUnitViewModel> UnitValidation)
        {
            _unitServices = unitServices;
            _unitValidation = UnitValidation;
        }

        [HttpGet("GetAllUnit")]
        public async Task<Response> GetAllUnit(int pageIndex = 0, int pageSize = 10) => await _unitServices.GetAllUnits(pageIndex, pageSize);

        [HttpPost("CreateUnit"), Authorize(policy: "AuthUser")]
        public async Task<IActionResult> CreateUnit(CreateUnitViewModel UnitModel) {
            if (ModelState.IsValid)
            {
                ValidationResult result = _unitValidation.Validate(UnitModel);
                if (result.IsValid)
                {
                    await _unitServices.CreateUnitAsync(UnitModel);
                }
                else
                {
                    var error = result.Errors.Select(x => x.ErrorMessage).ToList();
                    return BadRequest(error);
                }
            }
            return Ok("Create new Unit Success");
        }

        [HttpGet("ViewUnitById/{UnitId}")]
        public async Task<Response> GetUnitById(Guid UnitId) => await _unitServices.GetUnitById(UnitId);

        [HttpPut("UpdateUnit/{UnitId}"), Authorize(policy: "AuthUser")]
        public async Task<IActionResult> UpdateUnit(Guid UnitId, CreateUnitViewModel UnitModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _unitValidation.Validate(UnitModel);
                if (result.IsValid)
                {
                    await _unitServices.UpdateUnitAsync(UnitId, UnitModel);
                }
                else
                {
                    var error = result.Errors.Select(x => x.ErrorMessage).ToList();
                    return BadRequest(error);
                }
            }
            return Ok("Update Quizz Success");
        }

        [HttpGet("GetEnableUnits")]
        public async Task<Response> GetEnableUnits(int pageIndex = 0, int pageSize = 10) => await _unitServices.GetEnableUnitsAsync(pageIndex, pageSize);
        [HttpGet("GetDisableUnits")]
        public async Task<Response> GetDiableClasses(int pageIndex = 0, int pageSize = 10) => await _unitServices.GetDisableUnitsAsync(pageIndex, pageSize);
               
        [HttpGet("GetUnitsByModuleId/{ModuleId}")]
        public async Task<Response> GetUnitByModuleIdAsync(Guid ModuleId, int pageIndex = 0, int pageSize = 10)
        {
            return await _unitServices.GetUnitByModuleIdAsync(ModuleId, pageIndex, pageSize);
        }
        [HttpGet("GetUnitsByName/{UnitName}")]
        public async Task<Response> GetUnitByNameAsync(string UnitName, int pageIndex = 0, int pageSize = 10)
        {
            return await _unitServices.GetUnitByNameAsync(UnitName, pageIndex, pageSize);
        }
    }
}

