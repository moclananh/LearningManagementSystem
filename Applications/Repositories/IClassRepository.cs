using Domain.Entities;

namespace Applications.Repositories
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        Task<List<Class>> GetEnableClasses();
        Task<List<Class>> GetDisableClasses();
    }
}
