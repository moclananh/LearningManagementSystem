using Applications.Commons;
using Applications.ViewModels.AssignmentQuestionViewModels;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Http;

namespace Applications.Interfaces
{
    public interface IAssignmentQuestionService
    {
        public Task<Pagination<AssignmentQuestionViewModel>> GetAssignmentQuestionByAssignmentId(Guid AssignmentId, int pageIndex = 0, int pageSize = 10);
        Task<Response> UploadAssignmentQuestions(IFormFile formFile);
    }
}
