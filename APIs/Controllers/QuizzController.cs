using Application.ViewModels.QuizzViewModels;
using Applications.Interfaces;
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
    public class QuizzController : ControllerBase
    {
        private readonly IQuizzService _quizzServices;
        private readonly IValidator<CreateQuizzViewModel> _createQuizzValidator;
        private readonly IValidator<UpdateQuizzViewModel> _updateQuizzValidator;
        public QuizzController(IQuizzService quizzServices,
            IValidator<CreateQuizzViewModel> CreateQuizzValidator,
            IValidator<UpdateQuizzViewModel> UpdateQuizzValidator)
        {
            _quizzServices = quizzServices;
            _createQuizzValidator = CreateQuizzValidator;
            _updateQuizzValidator = UpdateQuizzValidator;
        }

        [HttpGet("GetAllQuizz")]
        public async Task<Response> GetAllQuizz(int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetAllQuizzes(pageIndex, pageSize);

        [HttpPost("CreateQuizz")]
        public async Task<IActionResult> CreateQuizz(CreateQuizzViewModel QuizzModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _createQuizzValidator.Validate(QuizzModel);
                if (result.IsValid)
                {
                    await _quizzServices.CreateQuizzAsync(QuizzModel);
                }
                else
                {
                    return BadRequest("Fail to create new Quizz");
                }
            }     
            return Ok("Create new Quizz Success");
        }

        [HttpGet("GetQuizzByQuizzId/{QuizzId}")]
        public async Task<Response> GetQuizzByQuizzId(Guid QuizzId) => await _quizzServices.GetQuizzByQuizzIdAsync(QuizzId);

        [HttpGet("GetQuizzByUnitId/{UnitId}")]
        public async Task<Response> GetQuizzByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetQuizzByUnitIdAsync(UnitId, pageIndex, pageSize);

        [HttpGet("GetQuizzByName/{QuizzName}")]
        public async Task<Response> GetQuizzesByName(string QuizzName, int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetQuizzByName(QuizzName, pageIndex, pageSize);

        [HttpGet("GetEnableQuizzes")]
        public async Task<Response> GetEnableQuizzes(int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetEnableQuizzes(pageIndex, pageSize);

        [HttpGet("GetDisableQuizzes")]
        public async Task<Response> GetDisableQuizzes(int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetDisableQuizzes(pageIndex, pageSize);

        [HttpPut("UpdateQuizz/{QuizzId}")]
        public async Task<IActionResult> UpdateQuizz(Guid QuizzId, UpdateQuizzViewModel updateQuizzView)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _updateQuizzValidator.Validate(updateQuizzView);
                if (result.IsValid)
                {
                    await _quizzServices.UpdatQuizzAsync(QuizzId, updateQuizzView);
                }
                else
                {
                    return BadRequest("Update Quizz Fail");
                }
            }
            return Ok("Update Quizz Success");
        }
    }
}
