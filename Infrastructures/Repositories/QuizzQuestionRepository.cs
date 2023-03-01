using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class QuizzQuestionRepository : GenericRepository<QuizzQuestion>, IQuizzQuestionRepository
    {
        private readonly AppDBContext _dbContext;
        public QuizzQuestionRepository(AppDBContext dbContext,
            ICurrentTime currentTime,
            IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<QuizzQuestion>> GetQuizzQuestionListByQuizzId(Guid QuizzId)
        {
            return await _dbContext.QuizzQuestions.Where(x => x.QuizzId == QuizzId).ToListAsync();
        }
    }
}
