using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    [EnableCors("AllowAll")]
    public class PracticeQuestionController : ControllerBase
    {
        private readonly IPracticeQuestionService _practicequestionService;
        public PracticeQuestionController(IPracticeQuestionService practicequestionService)
        {
            _practicequestionService = practicequestionService;
        }

        [HttpGet("ViewPracticeQuestionsById/{PracticeId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetPracticeQuestionByPracticeId(Guid PracticeId, int pageIndex = 0, int pageSize = 10) => await _practicequestionService.GetPracticeQuestionByPracticeId(PracticeId, pageIndex, pageSize);

        [HttpPost("UploadPracticeQuestionFile")]
        [Authorize(policy: "Admins")]
        public async Task<Response> UploadPracticeQuestionFile(IFormFile formFile) => await _practicequestionService.UploadPracticeQuestions(formFile);

        [HttpGet("{practiceId}/export")]
        [Authorize(policy: "All")]
        public async Task<IActionResult> Export(Guid practiceId)
        {
            var content = await _practicequestionService.ExportPracticeQuestionByPracticeId(practiceId);
            var fileName = $"PracticesQuestions_{practiceId}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpDelete("DeletePracticeQuestion/{startDate}/{endDate}/{PracticeId}")]
        [Authorize(policy: "Admins")]
        public async Task<Response> DeletePracticeQuestionByCreationDate(DateTime startDate, DateTime endDate, Guid PracticeId)
        {
            return await _practicequestionService.DeletePracticeQuestionByCreationDate(startDate, endDate, PracticeId);
        }
    }
}
