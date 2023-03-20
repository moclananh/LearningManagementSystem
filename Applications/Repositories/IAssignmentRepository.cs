using Applications.Commons;
using Domain.Entities;

namespace Applications.Repositories
{
    public interface IAssignmentRepository : IGenericRepository<Assignment>
    {
        Task<Assignment> GetAssignmentDetail(Guid AssignmentId);
        Task<Pagination<Assignment>> GetEnableAssignmentAsync(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Assignment>> GetDisableAssignmentAsync(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Assignment>> GetAssignmentByUnitId(Guid UnitId, int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Assignment>> GetAssignmentByName(string Name, int pageNumber = 0, int pageSize = 10);
    }
}
