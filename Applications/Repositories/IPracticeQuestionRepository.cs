using Applications.Commons;
using Domain.Entities;

namespace Applications.Repositories
{
    public interface IPracticeQuestionRepository : IGenericRepository<PracticeQuestion>
    {
        Task<Pagination<PracticeQuestion>> GetAllPracticeQuestionById(Guid practiceId, int pageIndex = 0, int pageSize = 10);
        Task UploadPracticeListAsync(List<PracticeQuestion> practiceQuestionList);
        Task<List<PracticeQuestion>> GetAllPracticeQuestionByPracticeId(Guid PracticeId);
    }
}
