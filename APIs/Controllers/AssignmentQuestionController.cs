using Applications.Interfaces;
using Applications.ViewModels.AssignmentQuestionViewModels;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<List<AssignmentQuestionViewModel>> GetAssignmentQuestionByAssignmentId(Guid AssignmentId) => await _assignmentquestionService.GetAssignmentQuestionByAssignmentId(AssignmentId);
    }
}
