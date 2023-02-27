using Applications.Commons;
using Applications.Interfaces;
using Applications.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class QuizzRepository : GenericRepository<Quizz>, IQuizzRepository
    {
        private readonly AppDBContext _dbContext;

        public QuizzRepository(AppDBContext appDBContext, 
            ICurrentTime currentTime, 
            IClaimService claimService) 
            : base(appDBContext, currentTime, claimService)
        {
            _dbContext = appDBContext;
        }

        public async Task<Pagination<Quizz>> GetDisableQuizzes(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Quizzs.CountAsync();
            var items = await _dbContext.Quizzs.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Quizz>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<Quizz>> GetEnableQuizzes(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Quizzs.CountAsync();
            var items = await _dbContext.Quizzs.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Quizz>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<Quizz>> GetQuizzByName(string QuizzName, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Quizzs.CountAsync();
            var items = await _dbContext.Quizzs.Where(x => x.QuizzName.Contains(QuizzName))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Quizz>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<Quizz>> GetQuizzByUnitIdAsync(Guid UnitId, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Quizzs.CountAsync();
            var items = await _dbContext.Quizzs.Where(x => x.UnitId.Equals(UnitId))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Quizz>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }
    }
}
