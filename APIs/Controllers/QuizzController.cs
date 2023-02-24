using Application.ViewModels.QuizzViewModels;
using Applications.Commons;
using Applications.Interfaces;
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

        [HttpGet("GetQuizzPagingsion")]
        public async Task<Pagination<QuizzViewModel>> GetQuizzPagingsion(int pageIndex = 0, int pageSize = 10)
        {
            return await _quizzServices.GetQuizzPagingsionAsync(pageIndex, pageSize);
        }

        [HttpPost("CreateQuizz")]
        public async Task<CreateQuizzViewModel> CreateQuizz(CreateQuizzViewModel QuizzModel) => await _quizzServices.CreateQuizzAsync(QuizzModel);

        [HttpGet("GetQuizzByQuizzId/{QuizzId}")]
        public async Task<QuizzViewModel> GetQuizzByQuizzId(Guid QuizzId) => await _quizzServices.GetQuizzByQuizzIdAsync(QuizzId);

        [HttpGet("GetQuizzByUnitId/{UnitId}")]
        public async Task<List<QuizzViewModel>> GetQuizzByUnitId(Guid UnitId) =>  await _quizzServices.GetQuizzByUnitIdAsync(UnitId);

        /*[HttpGet("GetQuizzByName/{QuizzName}")]
        public async Task<QuizzViewModel> GetQuizzByQuizzName(string QuizzName) => await _quizzServices.GetQuizzByName(QuizzName);*/

        [HttpGet("GetEnableQuizzes")]
        public async Task<List<QuizzViewModel>> GetEnableQuizzes() => await _quizzServices.GetEnableQuizzes();

        [HttpGet("GetDisableQuizzes")]
        public async Task<List<QuizzViewModel>> GetDisableQuizzes() => await _quizzServices.GetDisableQuizzes();

        [HttpPut("UpdateQuizz/{QizzId}")]
        public async Task<UpdateQuizzViewModel> UpdateQuizz(Guid QizzId, UpdateQuizzViewModel quizzModel) => await _quizzServices.UpdatQuizzAsync(QizzId, quizzModel);

    }
}
