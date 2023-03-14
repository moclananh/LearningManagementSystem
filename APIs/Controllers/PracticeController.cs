using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.PracticeViewModels;
using Applications.ViewModels.Response;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    public class PracticeController : ControllerBase
    {
        private readonly IPracticeService _service;
        private readonly IValidator<UpdatePracticeViewModel> _updatePracticeValidator;
        private readonly IValidator<CreatePracticeViewModel> _createPracticeValidator;
        public PracticeController(IPracticeService service, IValidator<UpdatePracticeViewModel> UpdatePracticeValidator, IValidator<CreatePracticeViewModel> CreatePracticeValidator)
        {
            _service = service;
            _updatePracticeValidator = UpdatePracticeValidator;
            _createPracticeValidator = CreatePracticeValidator;
        }
        [HttpGet("GetPracticesByUnitId/{UnitId}")]
        public async Task<Pagination<PracticeViewModel>> GetPracticesByUnitId(Guid UnitId) => await _service.GetPracticeByUnitId(UnitId);

        [HttpGet("GetPracticeById/{PracticeId}")]
        public async Task<PracticeViewModel> GetPracticeById(Guid PracticeId) => await _service.GetPracticeById(PracticeId);

        [HttpGet("GetAllPractice")]
        public async Task<Response> GetAllPractice(int pageIndex = 0, int pageSize = 10) => await _service.GetAllPractice(pageIndex, pageSize);

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
        public async Task<Response> GetPracticeByName(string PracticeName, int pageIndex = 0, int pageSize = 10) => await _service.GetPracticeByName(PracticeName, pageIndex, pageSize);
        [HttpGet("GetEnablePractice")]
        public async Task<Pagination<PracticeViewModel>> GetEnablePractices(int pageIndex = 0, int pageSize = 10) => await _service.GetEnablePractice(pageIndex, pageSize);
        [HttpGet("GetDisablePractice")]
        public async Task<Pagination<PracticeViewModel>> GetDisablePractice(int pageIndex = 0, int pageSize = 10) => await _service.GetDisablePractice(pageIndex, pageSize);
        [HttpPut("UpdatePractice/{PracticeId}")]
        public async Task<IActionResult> UpdatePractice(Guid PracticeId, UpdatePracticeViewModel practiceDTO)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _updatePracticeValidator.Validate(practiceDTO);
                if (result.IsValid)
                {
                    await _service.UpdatePractice(PracticeId, practiceDTO);
                }
                else
                {
                    return BadRequest("Update Practice Fail");
                }
            }
            return Ok("Update Practice Success");
        }
    }
}


