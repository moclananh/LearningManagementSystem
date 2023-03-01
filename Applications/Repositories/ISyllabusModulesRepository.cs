using Domain.EntityRelationship;

namespace Applications.Repositories
{
    public interface ISyllabusModulesRepository : IGenericRepository<SyllabusModule>
    {
        Task<SyllabusModule> GetSyllabusModules(Guid SyllabusId, Guid ModuleId);
    }
}
