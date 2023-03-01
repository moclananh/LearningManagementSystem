using Application.ViewModels.QuizzViewModels;
using Applications.Commons;
using Applications.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzController : ControllerBase
    {
        private readonly IQuizzServices _quizzServices;
        private readonly IValidator<CreateQuizzViewModel> _createQuizzValidator;
        private readonly IValidator<UpdateQuizzViewModel> _updateQuizzValidator;
        public QuizzController(IQuizzServices quizzServices,
            IValidator<CreateQuizzViewModel> CreateQuizzValidator,
            IValidator<UpdateQuizzViewModel> UpdateQuizzValidator)
        {
            _quizzServices = quizzServices;
            _createQuizzValidator = CreateQuizzValidator;
            _updateQuizzValidator = UpdateQuizzValidator;
        }

        [HttpGet("GetAllQuizz")]
        public async Task<Pagination<QuizzViewModel>> GetAllQuizz(int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetAllQuizzes(pageIndex, pageSize);

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
        public async Task<QuizzViewModel> GetQuizzByQuizzId(Guid QuizzId) => await _quizzServices.GetQuizzByQuizzIdAsync(QuizzId);

        [HttpGet("GetQuizzByUnitId/{UnitId}")]
        public async Task<Pagination<QuizzViewModel>> GetQuizzByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetQuizzByUnitIdAsync(UnitId, pageIndex, pageSize);

        [HttpGet("GetQuizzByName/{QuizzName}")]
        public async Task<Pagination<QuizzViewModel>> GetQuizzesByName(string QuizzName, int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetQuizzByName(QuizzName, pageIndex, pageSize);

        [HttpGet("GetEnableQuizzes")]
        public async Task<Pagination<QuizzViewModel>> GetEnableQuizzes(int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetEnableQuizzes(pageIndex, pageSize);

        [HttpGet("GetDisableQuizzes")]
        public async Task<Pagination<QuizzViewModel>> GetDisableQuizzes(int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetDisableQuizzes(pageIndex, pageSize);

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
