using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class OutputStandardRepository : GenericRepository<OutputStandard>, IOutputStandardRepository
    {
        private readonly AppDBContext _dbContext;
        public OutputStandardRepository(AppDBContext dbContext,
            ICurrentTime currentTime,
            IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
        }

    }
}
