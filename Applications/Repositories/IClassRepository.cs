using Applications.Commons;
using Domain.Entities;

namespace Applications.Repositories
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        Task<Pagination<Class>> GetEnableClasses(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Class>> GetDisableClasses(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Class>> GetClassByName(string Name, int pageNumber = 0, int pageSize = 10);
    }
}
