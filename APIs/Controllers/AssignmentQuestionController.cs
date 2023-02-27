using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.AssignmentQuestionViewModels;
using Applications.ViewModels.Response;
using ClosedXML.Excel;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentQuestionController : ControllerBase
    {
        private readonly IAssignmentQuestionService _assignmentquestionService;
        public AssignmentQuestionController(IAssignmentQuestionService assignmentQuestionService)
        {
            _assignmentquestionService = assignmentQuestionService;
        }

        [HttpGet("ViewAssignmentQuestionsByAssignmentId/{AssignmentId}")]
        public async Task<Pagination<AssignmentQuestionViewModel>> GetAssignmentQuestionByAssignmentId(Guid AssignmentId, int pageIndex = 0, int pageSize = 10) => await _assignmentquestionService.GetAssignmentQuestionByAssignmentId(AssignmentId, pageIndex, pageSize);

        [HttpPost("UploadAssignmentQuestionFile")]
        public async Task<Response> UploadAssignmentQuestions(IFormFile formFile) => await _assignmentquestionService.UploadAssignmentQuestions(formFile);

        [HttpGet("ExportAssignmentQuestionByAssignmentId/{AssId}")]
        public async Task<IActionResult> ExportAssignmentQuestionByAssignmentId(Guid AssId)
        {
            var questions = await _assignmentquestionService.GetAssignmentQuestionByAssignmentId(AssId);
            
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Assignment Questions");
            
            worksheet.Cell(1, 1).Value = "Note";
            worksheet.Cell(1, 2).Value = "Question";
            worksheet.Cell(1, 3).Value = "Answer";
            
            for (var i = 0; i < questions.Count; i++)
            {
                var question = questions[i];
                worksheet.Cell(i + 2, 1).Value = question.Note;
                worksheet.Cell(i + 2, 2).Value = question.Question;
                worksheet.Cell(i + 2, 3).Value = question.Answer;
            }
            
            var fileName = $"AssignmentQuestions_{AssId}.xlsx";
            var fileContent = new MemoryStream();
            workbook.SaveAs(fileContent);
            fileContent.Seek(0, SeekOrigin.Begin);
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
