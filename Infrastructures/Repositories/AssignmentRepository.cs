using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
    {
        private readonly AppDBContext _dbContext;
        public AssignmentRepository(AppDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Assignment>> GetAssignmentByUnitId(Guid UnitId)
        {
            return await _dbContext.Assignments.Where(a => a.UnitId == UnitId).ToListAsync();
        }

        public async Task<List<Assignment>> GetDisableAssignmentAsync()
        {
            return await _dbContext.Assignments.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable).ToListAsync();
        }

        public async Task<List<Assignment>> GetEnableAssignmentAsync()
        {
            return await _dbContext.Assignments.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable).ToListAsync();
        }
    }
}
