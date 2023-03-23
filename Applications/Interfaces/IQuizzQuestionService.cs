using Applications.Commons;
using Applications.ViewModels.QuizzQuestionViewModels;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Http;

namespace Applications.Interfaces
{
    public interface IQuizzQuestionService
    {
        Task<byte[]> ExportQuizzQuestionByQuizzId(Guid quizzId);
        public Task<Pagination<QuizzQuestionViewModel>> GetQuizzQuestionByQuizzId(Guid QuizzId, int pageIndex = 0, int pageSize = 10);
        public Task<QuizzQuestionViewModel> AddQuestion(QuizzQuestionViewModel question);
        public Task<QuizzQuestionViewModel> UpdateQuestion(Guid QuizzQuestionId, QuizzQuestionViewModel question);
        public Task<Response> UploadQuizzQuestion(IFormFile formFile);
        public Task<Response> DeleteQuizzQuestionByCreationDate(DateTime startDate, DateTime endDate, Guid QuizzId);
    }
}
