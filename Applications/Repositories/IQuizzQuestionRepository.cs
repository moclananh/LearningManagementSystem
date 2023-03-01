using Domain.Entities;

namespace Applications.Repositories
{
    public interface IQuizzQuestionRepository : IGenericRepository<QuizzQuestion>
    {
        Task<List<QuizzQuestion>> GetQuizzQuestionListByQuizzId(Guid QuizzId);
    }
}
