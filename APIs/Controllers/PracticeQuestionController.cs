using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.PracticeQuestionViewModels;
using Applications.ViewModels.Response;
using ClosedXML.Excel;
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


        [HttpGet("ExportPracticeQuestionByPracticeId/{PractId}")]
        public async Task<IActionResult> ExportPracticeQuestionByPracticeId(Guid PractId)
        {
            var pratices = await _practicequestionService.PracticeQuestionByPracticeId(PractId);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Practice Questions");

            worksheet.Cell(1, 1).Value = "Question";
            worksheet.Cell(1, 2).Value = "Answer";
            worksheet.Cell(1, 3).Value = "Note";

            for (var i = 0; i < pratices.Count; i++)
            {
                var question = pratices[i];
                worksheet.Cell(i + 2, 1).Value = question.Question;
                worksheet.Cell(i + 2, 2).Value = question.Answer;
                worksheet.Cell(i + 2, 3).Value = question.Note;
            }
            
            var fileName = $"AssignmentQuestions_{PractId}.xlsx";
            var fileContent = new MemoryStream();
            workbook.SaveAs(fileContent);
            fileContent.Seek(0, SeekOrigin.Begin);
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
