using Application.ViewModels.UnitViewModels;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using FluentValidation;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    [EnableCors("AllowAll")]
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
        [Authorize(policy: "All")]
        public async Task<Response> GetAllUnit(int pageIndex = 0, int pageSize = 10) => await _unitServices.GetAllUnits(pageIndex, pageSize);

        [HttpPost("CreateUnit")]
        //[Authorize(policy: "Admins")]
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
        [Authorize(policy: "All")]
        public async Task<Response> GetUnitById(Guid UnitId) => await _unitServices.GetUnitById(UnitId);

        [HttpPut("UpdateUnit/{UnitId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> UpdateUnit(Guid UnitId, CreateUnitViewModel UnitModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _unitValidation.Validate(UnitModel);
                if (result.IsValid)
                {
                    if (await _unitServices.UpdateUnitAsync(UnitId, UnitModel) is object)
                    {
                        return Ok("Update Success");
                    }
                    return BadRequest("Invalid Id");
                }
            }
            return BadRequest("Update Failed, Invalid Input Information");
        }

        [HttpGet("GetEnableUnits")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetEnableUnits(int pageIndex = 0, int pageSize = 10) => await _unitServices.GetEnableUnitsAsync(pageIndex, pageSize);
        [HttpGet("GetDisableUnits")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetDiableClasses(int pageIndex = 0, int pageSize = 10) => await _unitServices.GetDisableUnitsAsync(pageIndex, pageSize);
               
        [HttpGet("GetUnitsByModuleId/{ModuleId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetUnitByModuleIdAsync(Guid ModuleId, int pageIndex = 0, int pageSize = 10)
        {
            return await _unitServices.GetUnitByModuleIdAsync(ModuleId, pageIndex, pageSize);
        }
        [HttpGet("GetUnitsByName/{UnitName}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetUnitByNameAsync(string UnitName, int pageIndex = 0, int pageSize = 10)
        {
            return await _unitServices.GetUnitByNameAsync(UnitName, pageIndex, pageSize);
        }
    }
}

