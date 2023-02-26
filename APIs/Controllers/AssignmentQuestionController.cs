﻿using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.AssignmentQuestionViewModels;
using Applications.ViewModels.Response;
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
        public async Task<Pagination<AssignmentQuestionViewModel>> GetAssignmentQuestionByAssignmentId(Guid AssignmentId, int pageIndex = 0, int pageSize = 10) => await _assignmentquestionService.GetAssignmentQuestionByAssignmentId(AssignmentId, pageIndex, pageSize);

        [HttpPost("UploadAssignmentQuestionFile")]
        public async Task<Response> UploadAssignmentQuestions(IFormFile formFile) => await _assignmentquestionService.UploadAssignmentQuestions(formFile);
    }
}
