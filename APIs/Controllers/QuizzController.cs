using Application.ViewModels.QuizzViewModels;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    [EnableCors("AllowAll")]
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
        [Authorize(policy: "All")]
        public async Task<Response> GetAllQuizz(int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetAllQuizzes(pageIndex, pageSize);

        [HttpPost("CreateQuizz")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> CreateQuizz(CreateQuizzViewModel QuizzModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult check = _createQuizzValidator.Validate(QuizzModel);
                if (check.IsValid)
                {
                     await _quizzServices.CreateQuizzAsync(QuizzModel);
                }
                else
                {
                    var error = check.Errors.Select(x => x.ErrorMessage).ToList();
                    return BadRequest(error);
                }
            }
            return Ok("Create new Success");
        }

        [HttpGet("GetQuizzByQuizzId/{QuizzId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetQuizzByQuizzId(Guid QuizzId) => await _quizzServices.GetQuizzByQuizzIdAsync(QuizzId);

        [HttpGet("GetQuizzByUnitId/{UnitId}")]
        [Authorize(policy: "All")] 
        public async Task<Response> GetQuizzByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetQuizzByUnitIdAsync(UnitId, pageIndex, pageSize);

        [HttpGet("GetQuizzByName/{QuizzName}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetQuizzesByName(string QuizzName, int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetQuizzByName(QuizzName, pageIndex, pageSize);

        [HttpGet("GetEnableQuizzes")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetEnableQuizzes(int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetEnableQuizzes(pageIndex, pageSize);

        [HttpGet("GetDisableQuizzes")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetDisableQuizzes(int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetDisableQuizzes(pageIndex, pageSize);

        [HttpPut("UpdateQuizz/{QuizzId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> UpdateQuizz(Guid QuizzId, UpdateQuizzViewModel updateQuizzView)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _updateQuizzValidator.Validate(updateQuizzView);
                if (result.IsValid)
                {
                    if (await _quizzServices.UpdateQuizzAsync(QuizzId, updateQuizzView) != null)
                    {
                        return Ok("Update Assignment Success");
                    }
                    return BadRequest("Invalid Quizz Id");
                }
            }
            return BadRequest("Update Failed,Invalid Input Information");
        }
    }
}
