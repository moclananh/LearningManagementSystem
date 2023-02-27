﻿using Applications.ViewModels.OutputStandardViewModels;
using Applications.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Applications.Commons;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutputStandardController : ControllerBase
    {
        private readonly IOutputStandardService _outputStandardServices;
        public OutputStandardController(IOutputStandardService outputStandardServices)
        {
            _outputStandardServices = outputStandardServices;
        }
        [HttpGet("GetAllOutputStandard")]
        public async Task<List<OutputStandardViewModel>> ViewAllOutputStandardAsync() => await _outputStandardServices.ViewAllOutputStandardAsync();

        [HttpPost("CreateOutputStandard")]
        public async Task<CreateOutputStandardViewModel> CreateOutputStandard(CreateOutputStandardViewModel OutputStandardModel) => await _outputStandardServices.CreateOutputStandardAsync(OutputStandardModel);

        [HttpGet("GetOutputStandardByOutputStandardId/{OutputStandardId}")]
        public async Task<OutputStandardViewModel> GetOutputStandardByOutputStandardId(Guid OutputStandardId) => await _outputStandardServices.GetOutputStandardByOutputStandardIdAsync(OutputStandardId);

        [HttpPut("UpdateOutputStandard/{OutputStandardId}")]
        public async Task<UpdateOutputStandardViewModel> UpdateOutputStandard(Guid OutputStandardId, UpdateOutputStandardViewModel outputStandardModel) => await _outputStandardServices.UpdatOutputStandardAsync(OutputStandardId, outputStandardModel);
        [HttpGet("GetOutputStandardBySyllabusId/{SyllabusId}")]
        public async Task<Pagination<OutputStandardViewModel>> GetOutputStandardBySyllabusId(Guid SyllabusId, int pageIndex = 0, int pageSize = 10) => await _outputStandardServices.GetOutputStandardBySyllabusIdAsync(SyllabusId, pageIndex, pageSize);
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
