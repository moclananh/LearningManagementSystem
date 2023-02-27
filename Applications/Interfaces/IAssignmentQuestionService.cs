using Applications.Commons;
using Applications.ViewModels.AssignmentQuestionViewModels;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace Applications.Interfaces
{
    public interface IAssignmentQuestionService
    {
        public Task<Pagination<AssignmentQuestionViewModel>> GetAssignmentQuestionByAssignmentId(Guid AssignmentId, int pageIndex = 0, int pageSize = 10);
        Task<Response> UploadAssignmentQuestions(IFormFile formFile);
        Task<List<AssignmentQuestionViewModel>> GetAssignmentQuestionByAssignmentId(Guid assignmentId);
        Task<byte[]> ExportAssignmentQuestionByAssignmentId(Guid assignmentId);
    }
}
