using Applications.Commons;
using Applications.ViewModels.QuizzQuestionViewModels;

namespace Applications.Interfaces
{
    public interface IQuizzQuestionService
    {
        Task<byte[]> ExportQuizzQuestionByQuizzId(Guid quizzId);
    }
}
