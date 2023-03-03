using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.ModuleViewModels;
using Applications.ViewModels.Response;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> CreateModule(CreateModuleViewModel moduleModel)
        {
            if(ModelState.IsValid)
            {
                ValidationResult result = _validatorCreate.Validate(moduleModel);
                if (result.IsValid)
                {
                    await _moduleServices.CreateModule(moduleModel);
                }
                else
                {
                    return BadRequest("Fail to create new Module!");
                }
            }
            return Ok("Create Module Successfully");
        }

        [HttpGet("GetAllModules")]
        public async Task<Response> GetAllModules(int pageIndex = 0, int pageSize = 10) => await _moduleServices.GetAllModules(pageIndex,pageSize);

        [HttpGet("GetModulesBySyllabusId/{syllabusId}")]
        public async Task<Response> GetModulesBySyllabusId(Guid syllabusId, int pageIndex = 0, int pageSize = 10) => await _moduleServices.GetModulesBySyllabusId(syllabusId, pageIndex, pageSize);

        [HttpGet("GetModulesByName/{ModuleName}")]
        public async Task<Response> GetModulesByName(string ModuleName, int pageIndex = 0, int pageSize = 10) => await _moduleServices.GetModulesByName(ModuleName, pageIndex, pageSize);

        [HttpPut("UpdateModule")]
        public async Task<IActionResult> UpdateModule(Guid moduleId, UpdateModuleViewModel module)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validateUpdate.Validate(module);
                if(result.IsValid)
                {
                    await _moduleServices.UpdateModule(moduleId, module);
                }
                else
                {
                    return BadRequest("Fail to update !");
                }
            }
            return Ok("Update Module successfully");
        }

        [HttpGet("GetEnableModules")]
        public async Task<Response> GetEnableModules(int pageIndex = 0, int pageSize = 10) => await _moduleServices.GetEnableModules(pageIndex,pageSize);

        [HttpGet("GetDisableModules")]
        public async Task<Response> GetDisableModules(int pageIndex = 0, int pageSize = 10) => await _moduleServices.GetDisableModules(pageIndex,pageSize);

        [HttpPost("AddModuleUnit/{moduleId}/{unitId}")]
        public async Task<IActionResult> AddModuleUnit(Guid moduleId, Guid unitId)
        {
            if (ModelState.IsValid)
            {
                await _moduleServices.AddUnitToModule(moduleId, unitId);
                return Ok("Add Success");
            }
            return BadRequest("Add Fail");
        }
        [HttpDelete("DeleteUnit/{moduleId}/{unitId}")]
        public async Task<IActionResult> DeleteUnit(Guid moduleId, Guid unitId)
        {
            if (ModelState.IsValid)
            {
                await _moduleServices.RemoveUnitToModule(moduleId, unitId);
                return Ok("Remove Success");
            }
            return BadRequest("Remove Unit Fail");
        }
    }
}
