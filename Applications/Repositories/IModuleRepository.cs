using Applications.Commons;
using Domain.Entities;

namespace Applications.Repositories
{
    public interface IModuleRepository : IGenericRepository<Module>
    {
        Task<Pagination<Module>> GetModulesBySyllabusId(Guid syllabusId, int pageIndex = 0, int pageSize = 10);
        Task<Pagination<Module>> GetEnableModules(int pageIndex = 0, int pageSize = 10);
        Task<Pagination<Module>> GetDisableModules(int pageIndex = 0, int pageSize = 10);
        Task<Pagination<Module>> GetModuleByName(string name, int pageIndex = 0, int pageSize = 10);
    }
}
