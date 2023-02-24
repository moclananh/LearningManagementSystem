using Domain.Entities;

namespace Applications.Repositories
{
    public interface IAssignmentRepository : IGenericRepository<Assignment>
    {
        Task<List<Assignment>> GetEnableAssignmentAsync();
        Task<List<Assignment>> GetDisableAssignmentAsync();
        Task<List<Assignment>> GetAssignmentByUnitId(Guid UnitId);
        Task<List<Assignment>> GetAssignmentByName(string Name);
    }
}
