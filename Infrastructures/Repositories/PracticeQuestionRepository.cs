﻿using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class PracticeQuestionRepository : GenericRepository<PracticeQuestion>, IPracticeQuestionRepository
    {
        private readonly AppDBContext _dbContext;
        public PracticeQuestionRepository(AppDBContext dbContext,
            ICurrentTime currentTime,
            IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
        }

        public async Task<Pagination<PracticeQuestion>> GetAllPracticeQuestionById(Guid practiceId, int pageIndex = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Practices.CountAsync();
            var items = await _dbSet.Where(x => x.PracticeId.Equals(practiceId))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageIndex * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<PracticeQuestion>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task UploadPracticeListAsync(List<PracticeQuestion> practiceQuestionList)
        {
            await _dbContext.AddRangeAsync(practiceQuestionList);
        }
    }
}