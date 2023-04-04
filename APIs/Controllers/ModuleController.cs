using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.ModuleViewModels;
using Applications.ViewModels.Response;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    [EnableCors("AllowAll")]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleServices;
        private readonly IValidator<CreateModuleViewModel> _validatorCreate;
        private readonly IValidator<UpdateModuleViewModel> _validateUpdate;
        public ModuleController(IModuleService moduleServices,
                    IValidator<CreateModuleViewModel> validatorCreate,
                    IValidator<UpdateModuleViewModel> validatorUpdate)
        {
            _moduleServices = moduleServices;
            _validatorCreate = validatorCreate;
            _validateUpdate = validatorUpdate;
        }

        [HttpPost("CreateModule")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> CreateModule(CreateModuleViewModel moduleModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validatorCreate.Validate(moduleModel);
                if (result.IsValid)
                {
                    await _moduleServices.CreateModule(moduleModel);
                }
                else
                {
                    var error = result.Errors.Select(x => x.ErrorMessage).ToList();
                    return BadRequest("Fail to create new Module!");
                }
            }
            return Ok("Create Module Successfully");
        }

        [HttpGet("GetAllModules")]
        [Authorize(policy: "All")]
        public async Task<Response> GetAllModules(int pageIndex = 0, int pageSize = 10) => await _moduleServices.GetAllModules(pageIndex, pageSize);

        [HttpGet("GetModulesBySyllabusId/{syllabusId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetModulesBySyllabusId(Guid syllabusId, int pageIndex = 0, int pageSize = 10) => await _moduleServices.GetModulesBySyllabusId(syllabusId, pageIndex, pageSize);

        [HttpGet("GetModulesByName/{ModuleName}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetModulesByName(string ModuleName, int pageIndex = 0, int pageSize = 10) => await _moduleServices.GetModulesByName(ModuleName, pageIndex, pageSize);

        [HttpPut("UpdateModule")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> UpdateModule(Guid moduleId, UpdateModuleViewModel module)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validateUpdate.Validate(module);
                if (result.IsValid)
                {
                    if (await _moduleServices.UpdateModule(moduleId, module) != null)
                    {
                        return Ok("Update Module Succeed");
                    }
                    return BadRequest("Invalid Module Id");
                }
            }
            return BadRequest("Updated Failed, Invalid Input Information");
        }

        [HttpGet("GetEnableModules")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetEnableModules(int pageIndex = 0, int pageSize = 10) => await _moduleServices.GetEnableModules(pageIndex, pageSize);

        [HttpGet("GetDisableModules")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetDisableModules(int pageIndex = 0, int pageSize = 10) => await _moduleServices.GetDisableModules(pageIndex, pageSize);

        [HttpPost("AddModuleUnit/{moduleId}/{unitId}")]
         [Authorize(policy: "Admins")]
        public async Task<IActionResult> AddModuleUnit(Guid moduleId, Guid unitId)
        {
            if (ModelState.IsValid)
            {
                await _moduleServices.AddUnitToModule(moduleId, unitId);
                return Ok("Add Success");
            }
            return BadRequest("Add Fail");
        }

        [HttpPost("AddMultipleUnitToModule/{moduleId}")]
        [Authorize(policy: "Admins")]
        public async Task<Response> AddMultipleUnittoModule(Guid moduleId, List<Guid> unitId)
        {
            return await _moduleServices.AddMultipleUnitToModule(moduleId, unitId);
        }

        [HttpDelete("DeleteUnit/{moduleId}/{unitId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> DeleteUnitModule(Guid moduleId, Guid unitId)
        {
            if (ModelState.IsValid)
            {
                var deletedModuleUnit = await _moduleServices.RemoveUnitToModule(moduleId, unitId);
                if (deletedModuleUnit == null)
                {
                    return BadRequest("Unit not found in Module");
                }
                return Ok("Remove Success");

            }
            return BadRequest("Remove UnitModule Fail");
        }

    }
}


