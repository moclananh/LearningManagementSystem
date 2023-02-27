using Applications.Commons;
using Domain.Entities;

namespace Applications.Repositories
{
    public interface IUnitRepository : IGenericRepository<Unit>
    {
        Task<Pagination<Unit>> GetEnableUnits(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Unit>> GetDisableUnits(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Unit>> ViewAllUnitByModuleIdAsync(Guid ModuleId, int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Unit>> GetUnitByNameAsync(string UnitName, int pageNumber = 0, int pageSize = 10);
    }
}
