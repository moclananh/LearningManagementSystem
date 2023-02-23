using Domain.Entities;

namespace Applications.Repositories
{
    public interface IUnitRepository : IGenericRepository<Unit>
    {
        Task<List<Unit>> GetEnableUnits();
        Task<List<Unit>> GetDisableUnits();
    }
}
