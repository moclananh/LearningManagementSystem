using Applications.Commons;
using Applications.Interfaces;
using Applications.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class QuizzRepository : GenericRepository<Quizz>, IQuizzRepository
    {
        private readonly AppDBContext _context;

        public QuizzRepository(AppDBContext appDBContext, ICurrentTime currentTime, IClaimService claimService) : base(appDBContext, currentTime, claimService)
        {
            _context = appDBContext;
        }

        public async Task<Pagination<Quizz>> GetDisableQuizzes(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _context.Quizzs.CountAsync();
            var items = await _dbSet.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
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
            var itemCount = await _context.Quizzs.CountAsync();
            var items = await _dbSet.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
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

        public async Task<Pagination<Quizz>> GetQuizzByName(string Name, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _context.Quizzs.CountAsync();
            var items = await _dbSet.Where(x => x.QuizzName.Contains(Name))
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

        public Task<Pagination<Quizz>> GetQuizzByUnitIdAsync(Guid UnitId)
        {
            throw new NotImplementedException();
        }
        /*        public async Task<Pagination<Quizz>> GetQuizzByUnitIdAsync(Guid UnitId, int pageNumber = 0, int pageSize = 10) => await _context.Quizzs.Where(p2 => p2.UnitId.Equals(UnitId)).ToListAsync();
       public async Task<Pagination<Quizz>> GetQuizzByName(string Name, int pageNumber = 0, int pageSize = 10) => await _context.Quizzs.Where(x => x.QuizzName.Contains(Name)).ToListAsync();
       public async Task<Pagination<Quizz>> GetDisableQuizzes(int pageNumber = 0, int pageSize = 10) => await _context.Quizzs.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable).ToListAsync();
       public async Task<Pagination<Quizz>> GetEnableQuizzes(int pageNumber = 0, int pageSize = 10) => await _context.Quizzs.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable).ToListAsync();*/

    }
}
