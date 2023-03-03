using Applications.ViewModels.OutputStandardViewModels;
using Applications.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Applications.Commons;
using FluentValidation.Results;
using FluentValidation;
using Applications.ViewModels.Response;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutputStandardController : ControllerBase
    {
        private readonly IOutputStandardService _outputStandardServices;
        private readonly IValidator<UpdateOutputStandardViewModel> _updateOutputStandardValidator;
        public OutputStandardController(IOutputStandardService outputStandardServices,
            IValidator<UpdateOutputStandardViewModel> UpdateOutputStandardValidator)
        {
            _outputStandardServices = outputStandardServices;
            _updateOutputStandardValidator = UpdateOutputStandardValidator;
        }
        [HttpGet("GetAllOutputStandard")]
        public async Task<List<OutputStandardViewModel>> ViewAllOutputStandardAsync() => await _outputStandardServices.ViewAllOutputStandardAsync();

        [HttpPost("CreateOutputStandard")]
        public async Task<CreateOutputStandardViewModel> CreateOutputStandard(CreateOutputStandardViewModel OutputStandardModel) => await _outputStandardServices.CreateOutputStandardAsync(OutputStandardModel);

        [HttpGet("GetOutputStandardByOutputStandardId/{OutputStandardId}")]
        public async Task<Response> GetOutputStandardByOutputStandardId(Guid OutputStandardId) => await _outputStandardServices.GetOutputStandardByOutputStandardIdAsync(OutputStandardId);

        [HttpPut("UpdateOutputStandard/{OutputStandardId}")]
        public async Task<IActionResult> UpdateOutputStandard(Guid OutputStandardId, UpdateOutputStandardViewModel updateOutputStandardView)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _updateOutputStandardValidator.Validate(updateOutputStandardView);
                if (result.IsValid)
                {
                    await _outputStandardServices.UpdatOutputStandardAsync(OutputStandardId, updateOutputStandardView);
                }
                else
                {
                    return BadRequest("Update OutputStandard Fail");
                }
            }
            return Ok("Update OutputStandard Success");
        }
        [HttpGet("GetOutputStandardBySyllabusId/{SyllabusId}")]
        public async Task<Response> GetOutputStandardBySyllabusId(Guid SyllabusId, int pageIndex = 0, int pageSize = 10) => await _outputStandardServices.GetOutputStandardBySyllabusIdAsync(SyllabusId, pageIndex, pageSize);
        [HttpPost("OutputStandard/AddOutputStandard/{SyllabusId}/{OutputStandardId}")]
        public async Task<IActionResult> AddOutputStandard(Guid SyllabusId, Guid OutputStandardId)
        {
            if (ModelState.IsValid)
            {
                await _outputStandardServices.AddOutputStandardToSyllabus(SyllabusId, OutputStandardId);
                return Ok("Add Success");
            }
            return BadRequest("Add OutputStandard Fail");
        }

        [HttpDelete("OutputStandard/DeleteOutputStandard/{SyllabusId}/{OutputStandardId}")]
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
