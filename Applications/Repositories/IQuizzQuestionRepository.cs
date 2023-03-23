using Applications.Commons;
using Domain.Entities;

namespace Applications.Repositories
{
    public interface IQuizzQuestionRepository : IGenericRepository<QuizzQuestion>
    {
        Task<List<QuizzQuestion>> GetQuizzQuestionListByQuizzId(Guid QuizzId);
        Task<Pagination<QuizzQuestion>> GetQuestionByQuizzId(Guid QuizzId, int pageIndex = 0, int pageSize = 10);
        Task<List<QuizzQuestion>> GetQuizzQuestionListByCreationDate(DateTime startDate, DateTime endDate, Guid QuizzId);
    }
}
