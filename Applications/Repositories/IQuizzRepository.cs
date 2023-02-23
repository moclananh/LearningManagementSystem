using Applications.Repositories;
using Domain.Entities;

namespace Applications.IRepositories
{
    public interface IQuizzRepository : IGenericRepository<Quizz>
    {
        Task<List<Quizz>> GetQuizzByUnitIdAsync(Guid UnitId);
        Task<List<Quizz>> GetQuizzByName(string Name);
        Task<List<Quizz>> GetEnableQuizzes();
        Task<List<Quizz>> GetDisableQuizzes();
    }
}
