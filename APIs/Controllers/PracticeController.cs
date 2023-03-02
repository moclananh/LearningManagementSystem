using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.PracticeViewModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticeController : ControllerBase
    {
        private readonly IPracticeService _service;
        private readonly IValidator<CreatePracticeViewModel> _createPracticeValidator;
        //private readonly IValidator<UpdatePracticeViewModel> _updatePracticeValidator;
        public PracticeController(IPracticeService service,
             IValidator<CreatePracticeViewModel> CreatePracticeValidator
           /* IValidator<UpdatePracticeViewModel> UpdatePracticeValidator*/)
        {
            _service = service;
            _createPracticeValidator = CreatePracticeValidator;
            //_updatePracticeValidator = UpdatePracticeValidator;
        }
        [HttpGet("GetPracticesByUnitId/{UnitId}")]
        public async Task<Pagination<PracticeViewModel>> GetPracticesByUnitId(Guid UnitId) => await _service.GetPracticeByUnitId(UnitId);

        [HttpGet("GetPracticeById/{PracticeId}")]
        public async Task<PracticeViewModel> GetPracticeById(Guid PracticeId) => await _service.GetPracticeById(PracticeId);

        [HttpGet("GetAllPractice")]
        public async Task<Pagination<PracticeViewModel>> GetAllPractice(int pageIndex = 0, int pageSize = 10) => await _service.GetAllPractice(pageIndex, pageSize);

        [HttpPost("CreatePractice")]
        public async Task<IActionResult> CreatePractice(CreatePracticeViewModel PracticeModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _createPracticeValidator.Validate(PracticeModel);
                if (result.IsValid)
                {
                    await _service.CreatePracticeAsync(PracticeModel);
                }
                else
                {
                    return BadRequest("Fail to create new Practice");
                }
            }
            return Ok("Create new Practice Success");
        }

        [HttpGet("GetPracticeByName/{PracticeName}")]
        public async Task<Pagination<PracticeViewModel>> GetPracticeByName(string PracticeName, int pageIndex = 0, int pageSize = 10) => await _service.GetpracticeByName(PracticeName, pageIndex, pageSize);
        [HttpGet("GetEnablePractice")]
        public async Task<Pagination<PracticeViewModel>> GetEnablePractices(int pageIndex = 0, int pageSize = 10) => await _service.GetEnablePractice(pageIndex, pageSize);
        [HttpGet("GetDisablePractice")]
        public async Task<Pagination<PracticeViewModel>> GetDisablePractice(int pageIndex = 0, int pageSize = 10) => await _service.GetDisablePractice(pageIndex, pageSize);
        [HttpPut("UpdatePractice/{PracticeId}")]
        public async Task<UpdatePracticeViewModel> UpdatePractice(Guid PracticeId, UpdatePracticeViewModel practiceDTO) => await _service.UpdatePractice(PracticeId, practiceDTO);
    }
}
