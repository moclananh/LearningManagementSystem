using Domain.Entities;

namespace Applications.Repositories
{
    public interface IModuleRepository : IGenericRepository<Module>
    {
        Task<List<Module>> GetModulesBySyllabusId(Guid syllabusId);
        Task<List<Module>> GetEnableModules();
        Task<List<Module>> GetDisableModules();
        Task<List<Module>> GetModuleByName(string name);
    }
}
