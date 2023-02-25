using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.PracticeQuestionViewModels;
using Applications.ViewModels.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticeQuestionController : ControllerBase
    {
        private readonly IPracticeQuestionService _practicequestionService;
        public PracticeQuestionController(IPracticeQuestionService practicequestionService)
        {
            _practicequestionService = practicequestionService;
        }

        [HttpGet("ViewPracticeQuestionsById/{PracticeId}")]
        public async Task<Pagination<PracticeQuestionViewModel>> GetPracticeQuestionByPracticeId(Guid PracticeId, int pageIndex = 0, int pageSize = 10) => await _practicequestionService.GetPracticeQuestionByPracticeId(PracticeId, pageIndex, pageSize);

        [HttpPost("UploadPracticeQuestionFile")]
        public async Task<Response> UploadPracticeQuestionFile(IFormFile formFile) => await _practicequestionService.UploadPracticeQuestions(formFile);
    }
}
