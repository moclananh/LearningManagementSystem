using Applications.ViewModels.AssignmentQuestionViewModels;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Http;

namespace Applications.Interfaces
{
    public interface IAssignmentQuestionService
    {
        public Task<List<AssignmentQuestionViewModel>> GetAssignmentQuestionByAssignmentId(Guid AssignmentId);
        Task<Response> UploadAssignmentQuestions(IFormFile formFile);
    }
}
