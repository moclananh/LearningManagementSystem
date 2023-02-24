using Applications.ViewModels.OutputStandardViewModels;
using Applications.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

    }
}
