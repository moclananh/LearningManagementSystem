using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.QuizzQuestionViewModels;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzQuestionController : ControllerBase
    {
        private readonly IQuizzQuestionService _quizzQuestionService;
        public QuizzQuestionController(IQuizzQuestionService quizzQuestionService)
        {
            _quizzQuestionService = quizzQuestionService;
        }
        [HttpGet("GetQuestionByQuizzId/{QuizzId}")]
        public async Task<Pagination<QuizzQuestionViewModel>> GetQuestionByQuizzId(Guid QuizzId, int pageIndex = 0, int pageSize = 10) => await _quizzQuestionService.GetQuizzQuestionByQuizzId(QuizzId, pageIndex, pageSize);
        [HttpPost("UploadQuizzQuestions")]
        public async Task<Response> UploadQuizzQuestion(IFormFile formFile) => await _quizzQuestionService.UploadQuizzQuestion(formFile);
        [HttpGet("ExportQuizzQuestion/{QuizzId}")]
        public async Task<IActionResult> ExportQuizzQuestion(Guid QuizzId)
        {
            var content = await _quizzQuestionService.ExportQuizzQuestionByQuizzId(QuizzId);

            var fileName = $"QuizzQuestions_{QuizzId}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
