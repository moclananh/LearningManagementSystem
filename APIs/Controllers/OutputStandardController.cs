using Applications.ViewModels.OutputStandardViewModels;
using Applications.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using FluentValidation;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Applications.Services;
using Domain.Entities;


namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutputStandardController : ControllerBase
    {
        private readonly IOutputStandardService _outputStandardServices;
        private readonly IValidator<UpdateOutputStandardViewModel> _updateOutputStandardValidator;
        private readonly IValidator<CreateOutputStandardViewModel> _createOutputStandardValidator;
        public OutputStandardController(IOutputStandardService outputStandardServices,
            IValidator<UpdateOutputStandardViewModel> UpdateOutputStandardValidator, IValidator<CreateOutputStandardViewModel> CreateOutputStandardValidator)
        {
            _outputStandardServices = outputStandardServices;
            _updateOutputStandardValidator = UpdateOutputStandardValidator;
            _createOutputStandardValidator = CreateOutputStandardValidator;
        }

        [HttpGet("GetAllOutputStandard")]
        public async Task<Response> GetAllOutputStandard(int pageIndex = 0, int pageSize = 10) => await _outputStandardServices.GetAllOutputStandardAsync(pageIndex, pageSize);

        [HttpPost("CreateOutputStandard"), Authorize(policy: "AuthUser")]
        public async Task<IActionResult> CreateOutputStandard(CreateOutputStandardViewModel OutputstandardModule)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _createOutputStandardValidator.Validate(OutputstandardModule);
                if (result.IsValid)
                {
                    await _outputStandardServices.CreateOutputStandardAsync(OutputstandardModule);
                }
                else
                {
                    var error = result.Errors.Select(x => x.ErrorMessage).ToList();
                    return BadRequest(error);
                }
            }
            return Ok("Create new OutputStandard Success");
        }
        [HttpGet("GetOutputStandardByOutputStandardId/{OutputStandardId}")]
        public async Task<Response> GetOutputStandardByOutputStandardId(Guid OutputStandardId) => await _outputStandardServices.GetOutputStandardByOutputStandardIdAsync(OutputStandardId);

        [HttpPut("UpdateOutputStandard/{OutputStandardId}"), Authorize(policy: "AuthUser")]
        public async Task<IActionResult> UpdateOutputStandard(Guid OutputStandardId, UpdateOutputStandardViewModel updateOutputStandardView)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _updateOutputStandardValidator.Validate(updateOutputStandardView);
                if (result.IsValid)
                {
                    if (await _outputStandardServices.UpdatOutputStandardAsync(OutputStandardId, updateOutputStandardView) != null)
                    {
                        return Ok("Update OutputStandard Success");
                    }
                    return BadRequest("Invalid OutputStandard Id");
                }
            }
            return Ok("Update OutputStandard Success");
        }

        [HttpGet("GetOutputStandardBySyllabusId/{SyllabusId}")]
        public async Task<Response> GetOutputStandardBySyllabusId(Guid SyllabusId, int pageIndex = 0, int pageSize = 10) => await _outputStandardServices.GetOutputStandardBySyllabusIdAsync(SyllabusId, pageIndex, pageSize);
        
        [HttpPost("OutputStandard/AddOutputStandardToSyllabus/{SyllabusId}/{OutputStandardId}"), Authorize(policy: "AuthUser")]
        public async Task<IActionResult> AddOutputStandard(Guid SyllabusId, Guid OutputStandardId)
        {
            if (ModelState.IsValid)
            {
                await _outputStandardServices.AddOutputStandardToSyllabus(SyllabusId, OutputStandardId);
                return Ok("Add Success");
            }
            return BadRequest("Add OutputStandard Fail");
        }

        [HttpDelete("OutputStandard/DeleteOutputStandard/{SyllabusId}/{OutputStandardId}"), Authorize(policy: "AuthUser")]
        public async Task<IActionResult> DeleteOutputStandard(Guid SyllabusId, Guid OutputStandardId)
        {
            if (ModelState.IsValid)
            {
                await _outputStandardServices.RemoveOutputStandardToSyllabus(SyllabusId, OutputStandardId);
                return Ok("Remove Success");
            }
            return BadRequest("Remove OutputStandard Fail");
        }
    }
}
