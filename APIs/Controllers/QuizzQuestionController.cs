using Applications;
using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.QuizzQuestionViewModels;
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
    public class QuizzQuestionController : ControllerBase
    {
        private readonly IQuizzQuestionService _quizzQuestionService;
        private readonly IUnitOfWork _unitOfWork;
        public QuizzQuestionController(IQuizzQuestionService quizzQuestionService, IUnitOfWork unitOfWork)
        {
            _quizzQuestionService = quizzQuestionService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetQuestionByQuizzId/{QuizzId}")]
        [Authorize(policy: "All")]
        public async Task<Pagination<QuizzQuestionViewModel>> GetQuestionByQuizzId(Guid QuizzId, int pageIndex = 0, int pageSize = 10) => await _quizzQuestionService.GetQuizzQuestionByQuizzId(QuizzId, pageIndex, pageSize);
        
        [HttpPost("UploadQuizzQuestions")]
        [Authorize(policy: "Admins")]
        public async Task<Response> UploadQuizzQuestion(IFormFile formFile) => await _quizzQuestionService.UploadQuizzQuestion(formFile);
        
        [HttpGet("ExportQuizzQuestion/{QuizzId}")]
        [Authorize(policy: "All")]
        public async Task<IActionResult> ExportQuizzQuestion(Guid QuizzId)
        {
            try
            {
                var content = await _quizzQuestionService.ExportQuizzQuestionByQuizzId(QuizzId);

                if (content == null || content.Length == 0)
                {
                    return NotFound();
                }

                var fileName = $"QuizzQuestions_{QuizzId}.xlsx";
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (ArgumentException ex)
            {
                // Return a bad request error
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception or return an error message to the client
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("DeleteQuizzQuestion/{startDate}/{endDate}/{QuizzId}")]
        [Authorize(policy: "Admins")]
        public async Task<Response> DeleteQuizzQuestionByCreationDate(DateTime startDate, DateTime endDate, Guid QuizzId)
        {
            return await _quizzQuestionService.DeleteQuizzQuestionByCreationDate(startDate, endDate, QuizzId);
        }
    }
}
