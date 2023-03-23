using Applications.Commons;
using Applications.ViewModels.AssignmentQuestionViewModels;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace Applications.Interfaces
{
    public interface IAssignmentQuestionService
    {
        public Task<Response> GetAssignmentQuestionByAssignmentId(Guid AssignmentId, int pageIndex = 0, int pageSize = 10);
        Task<Response> UploadAssignmentQuestions(IFormFile formFile);        
        Task<byte[]> ExportAssignmentQuestionByAssignmentId(Guid assignmentId);
        public Task<Response> DeleteAssignmentQuestionByCreationDate(DateTime startDate, DateTime endDate, Guid AssignmnentId);
    }
}
