using Applications.Interfaces;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    [EnableCors("AllowAll")]
    public class AssignmentQuestionController : ControllerBase
    {
        private readonly IAssignmentQuestionService _assignmentquestionService;
        public AssignmentQuestionController(IAssignmentQuestionService assignmentQuestionService)
        {
            _assignmentquestionService = assignmentQuestionService;
        }

        [HttpGet("ViewAssignmentQuestionsByAssignmentId/{AssignmentId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetAssignmentQuestionByAssignmentId(Guid AssignmentId, int pageIndex = 0, int pageSize = 10) => await _assignmentquestionService.GetAssignmentQuestionByAssignmentId(AssignmentId, pageIndex, pageSize);

        [HttpPost("UploadAssignmentQuestionFile")]
        [Authorize(policy: "Admins")]
        public async Task<Response> UploadAssignmentQuestions(IFormFile formFile) => await _assignmentquestionService.UploadAssignmentQuestions(formFile);

        [HttpGet("{assignmentId}/export")]
        [Authorize(policy: "All")]
        public async Task<IActionResult> Export(Guid assignmentId)
        {
            var content = await _assignmentquestionService.ExportAssignmentQuestionByAssignmentId(assignmentId);

            var fileName = $"AssignmentQuestions_{assignmentId}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpDelete("DeleteAssignmentQuestion/{startDate}/{endDate}/{AssignmentId}")]
        [Authorize(policy: "Admins")]
        public async Task<Response> DeleteAssignmentQuestionByCreationDate(DateTime startDate, DateTime endDate, Guid AssignmentId)
        {
            return await _assignmentquestionService.DeleteAssignmentQuestionByCreationDate(startDate, endDate, AssignmentId);
        }
    }
}
