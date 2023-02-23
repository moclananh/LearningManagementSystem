using Applications.Interfaces;
using Applications.ViewModels.ModuleViewModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleServices _moduleServices;
        private readonly IValidator<ModuleViewModels> _validator;
        public ModuleController(IModuleServices moduleServices, IValidator<ModuleViewModels> validator)
        {
            _moduleServices = moduleServices;
            _validator = validator;
        }

        [HttpPost("CreateModule")]
        public async Task<IActionResult> CreateModule(ModuleViewModels moduleModel)
        {
            if(ModelState.IsValid)
            {
                ValidationResult result = _validator.Validate(moduleModel);
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
        public async Task<List<ModuleViewModels>> GetAllModules() => await _moduleServices.GetAllModules();

        [HttpGet("GetModulesBySyllabusId/{syllabusId}")]
        public async Task<List<ModuleViewModels>> GetModulesBySyllabusId(Guid syllabusId) => await _moduleServices.GetModulesBySyllabusId(syllabusId);

        [HttpPut("UpdateModule")]
        public async Task<IActionResult> UpdateModule(Guid moduleId, ModuleViewModels module)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validator.Validate(module);
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
        public async Task<List<ModuleViewModels>> GetEnableModules() => await _moduleServices.GetEnableModules();

        [HttpGet("GetDisableModules")]
        public async Task<List<ModuleViewModels>> GetDisableModules() => await _moduleServices.GetDisableModules();
    }
}
