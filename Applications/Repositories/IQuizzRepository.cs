using Applications.Commons;
using Applications.Repositories;
using Domain.Entities;

namespace Applications.IRepositories
{
    public interface IQuizzRepository : IGenericRepository<Quizz>
    {
        Task<Pagination<Quizz>> GetQuizzByUnitIdAsync(Guid UnitId, int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Quizz>> GetQuizzByName(string QuizzName, int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Quizz>> GetEnableQuizzes(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Quizz>> GetDisableQuizzes(int pageNumber = 0, int pageSize = 10);
    }
}
