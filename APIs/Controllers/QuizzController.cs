using Application.ViewModels.QuizzViewModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.ClassViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzController : ControllerBase
    {
        private readonly IQuizzServices _quizzServices;
        public QuizzController(IQuizzServices quizzServices)
        {
            _quizzServices = quizzServices;
        }

        [HttpGet("GetAllQuizz")]
        public async Task<Pagination<QuizzViewModel>> GetQuizzPagingsion(int pageIndex = 0, int pageSize = 10)
        {
            return await _quizzServices.GetAllQuizzes(pageIndex, pageSize);
        }

        [HttpPost("CreateQuizz")]
        public async Task<CreateQuizzViewModel> CreateQuizz(CreateQuizzViewModel QuizzModel) => await _quizzServices.CreateQuizzAsync(QuizzModel);

        [HttpGet("GetQuizzByQuizzId/{QuizzId}")]
        public async Task<QuizzViewModel> GetQuizzByQuizzId(Guid QuizzId) => await _quizzServices.GetQuizzByQuizzIdAsync(QuizzId);

        [HttpGet("GetQuizzByUnitId/{UnitId}")]
        public async Task<Pagination<QuizzViewModel>> GetQuizzByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10) =>  await _quizzServices.GetQuizzByUnitIdAsync(UnitId, pageIndex, pageSize);

        [HttpGet("GetQuizzByName/{QuizzName}")]
        public async Task<Pagination<QuizzViewModel>> GetQuizzesByName(string QuizzName, int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetQuizzByName(QuizzName, pageIndex, pageSize);

        [HttpGet("GetEnableQuizzes")]
        public async Task<Pagination<QuizzViewModel>> GetEnableQuizzes(int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetEnableQuizzes(pageIndex, pageSize);

        [HttpGet("GetDisableQuizzes")]
        public async Task<Pagination<QuizzViewModel>> GetDisableQuizzes(int pageIndex = 0, int pageSize = 10) => await _quizzServices.GetDisableQuizzes(pageIndex, pageSize);

        [HttpPut("UpdateQuizz/{QizzId}")]
        public async Task<UpdateQuizzViewModel> UpdateQuizz(Guid QizzId, UpdateQuizzViewModel quizzModel) => await _quizzServices.UpdatQuizzAsync(QizzId, quizzModel);

    }
}
