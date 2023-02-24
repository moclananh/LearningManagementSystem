using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
    {
        private readonly AppDBContext _dbContext;
        public AssignmentRepository(AppDBContext dbContext, ICurrentTime currentTime, IClaimService claimService) : base(dbContext,currentTime,claimService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Assignment>> GetAssignmentByName(string Name) => await _dbContext.Assignments.Where(x => x.AssignmentName.Contains(Name)).ToListAsync();

        public async Task<List<Assignment>> GetAssignmentByUnitId(Guid UnitId) => await _dbContext.Assignments.Where(a => a.UnitId == UnitId).ToListAsync();

        public async Task<List<Assignment>> GetDisableAssignmentAsync() => await _dbContext.Assignments.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable).ToListAsync();

        public async Task<List<Assignment>> GetEnableAssignmentAsync() => await _dbContext.Assignments.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable).ToListAsync();
    }
}
