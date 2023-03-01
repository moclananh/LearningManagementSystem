using Applications.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzQuestionController : ControllerBase
    {
        private readonly IQuizzQuestionService _quizzquestionService;
        public QuizzQuestionController(IQuizzQuestionService quizzQuestionService)
        {
            _quizzquestionService = quizzQuestionService;
        }
        [HttpGet("{quizzId}/export")]
        public async Task<IActionResult> Export(Guid quizzId)
        {
            var content = await _quizzquestionService.ExportQuizzQuestionByQuizzId(quizzId);

            var fileName = $"QuizzQuestions_{quizzId}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
