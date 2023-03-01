using Application.ViewModels.UnitViewModels;
using Applications.Repositories;
using Applications.Commons;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using Application.ViewModels.QuizzViewModels;
using FluentValidation;
using Domain.Entities;

namespace API.Controllers
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
        public async Task<Pagination<UnitViewModel>> GetAllUnit(int pageIndex = 0, int pageSize = 10) => await _unitServices.GetAllUnits(pageIndex, pageSize);

        [HttpPost("CreateUnit")]
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
                    return BadRequest("Fail to create new Unit");
                }
            }
            return Ok("Create new Unit Success");
        }

        [HttpGet("ViewUnitById/{UnitId}")]
        public async Task<UnitViewModel> GetUnitById(Guid UnitId) => await _unitServices.ViewUnitById(UnitId);

        [HttpPut("UpdateUnit/{UnitId}")]
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
                    return BadRequest("Update Quizz Fail");
                }
            }
            return Ok("Update Quizz Success");
        }

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

