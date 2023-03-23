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
        public async Task<Pagination<QuizzQuestion>> GetQuestionByQuizzId(Guid QuizzId, int pageIndex = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.QuizzQuestions.Where(x => x.QuizzId == QuizzId).CountAsync();
            var items = await _dbSet.Where(x => x.QuizzId.Equals(QuizzId))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageIndex * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<QuizzQuestion>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<List<QuizzQuestion>> GetQuizzQuestionListByCreationDate(DateTime startDate, DateTime endDate, Guid QuizzId)
        {
            var itemCount = await _dbContext.QuizzQuestions.CountAsync();
            var items = await _dbContext.QuizzQuestions.Where(x => x.QuizzId == QuizzId && (x.CreationDate >= startDate && x.CreationDate <= endDate)).ToListAsync();
            var result = new List<QuizzQuestion>(items);
            return result;
        }

        public async Task<List<QuizzQuestion>> GetQuizzQuestionListByQuizzId(Guid QuizzId)
        {
            return await _dbContext.QuizzQuestions.Where(x => x.QuizzId == QuizzId).ToListAsync();
        }
    }
}
