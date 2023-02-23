using Applications.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class QuizzRepository : GenericRepository<Quizz>, IQuizzRepository
    {
        private readonly AppDBContext _context;

        public QuizzRepository(AppDBContext appDBContext) : base(appDBContext)
        {
            _context = appDBContext;
        }
        public async Task<List<Quizz>> GetQuizzByUnitIdAsync(Guid UnitId) => await _context.Quizzs.Where(p2 => p2.UnitId.Equals(UnitId)).ToListAsync();
        public async Task<List<Quizz>> GetQuizzByName(string Name) => await _context.Quizzs.Where(x => x.QuizzName.Contains(Name)).ToListAsync();
        public async Task<List<Quizz>> GetDisableQuizzes() => await _context.Quizzs.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable).ToListAsync();
        public async Task<List<Quizz>> GetEnableQuizzes() => await _context.Quizzs.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable).ToListAsync();

    }
}
