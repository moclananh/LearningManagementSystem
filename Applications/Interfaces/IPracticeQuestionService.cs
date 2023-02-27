using Applications.Commons;
using Applications.ViewModels.PracticeQuestionViewModels;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Http;

namespace Applications.Interfaces
{
    public interface IPracticeQuestionService
    {
        public Task<Pagination<PracticeQuestionViewModel>> GetPracticeQuestionByPracticeId(Guid PracticeId, int pageIndex = 0, int pageSize = 10);
        Task<Response> UploadPracticeQuestions(IFormFile formFile);
        Task<List<PracticeQuestionViewModel>> PracticeQuestionByPracticeId(Guid practiceId);
        Task<byte[]> ExportPracticeQuestionByPracticeId(Guid practiceId);
    }
}
