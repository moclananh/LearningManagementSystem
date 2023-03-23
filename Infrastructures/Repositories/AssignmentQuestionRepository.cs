using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class AssignmentQuestionRepository : GenericRepository<AssignmentQuestion>, IAssignmentQuestionRepository
    {
        private readonly AppDBContext _dbContext;
        public AssignmentQuestionRepository(AppDBContext dbContext,
            ICurrentTime currentTime,
            IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AssignmentQuestion>> GetAssignmentQuestionListByAssignmentId(Guid AssignmentId)
        {
            return await _dbContext.AssignmentQuestions.Where(x => x.AssignmentId == AssignmentId).ToListAsync();
        }

        public async Task<Pagination<AssignmentQuestion>> GetAllAssignmentQuestionByAssignmentId(Guid AssignmentId, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.AssignmentQuestions.CountAsync();
            var items = await _dbSet.Where(x => x.AssignmentId.Equals(AssignmentId))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<AssignmentQuestion>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task UploadAssignmentListAsync(List<AssignmentQuestion> assignmentQuestionList)
        {
            await _dbContext.AddRangeAsync(assignmentQuestionList);
        }

        public async Task<List<AssignmentQuestion>> GetAssignmentQuestionListByCreationDate(DateTime startDate, DateTime endDate, Guid AssignmentId)
        {
            var itemCount = await _dbContext.AssignmentQuestions.CountAsync();
            var items = await _dbContext.AssignmentQuestions.Where(x => x.AssignmentId == AssignmentId && (x.CreationDate >= startDate && x.CreationDate <= endDate)).ToListAsync();
            var result = new List<AssignmentQuestion>(items);
            return result;
        }
    }
}
