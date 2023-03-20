using Applications.Interfaces;
using Applications.Repositories;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class SyllabusOutputStandardRepository : GenericRepository<SyllabusOutputStandard>, ISyllabusOutputStandardRepository
    {
        private readonly AppDBContext _dbContext;
        public SyllabusOutputStandardRepository(AppDBContext appDBContext, ICurrentTime currentTime, IClaimService claimService) : base(appDBContext, currentTime, claimService)
        {
            _dbContext = appDBContext;
        }

        public async Task<SyllabusOutputStandard> GetSyllabusOutputStandard(Guid SyllabusId, Guid OutputStandardId) => await _dbContext.SyllabusOutputStandard.FirstOrDefaultAsync(x => x.SyllabusId == SyllabusId && x.OutputStandardId == OutputStandardId);

    }
}
