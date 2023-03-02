using Domain.EntityRelationship;


namespace Applications.Repositories
{
    public interface ISyllabusModuleRepository : IGenericRepository<SyllabusModule>
    {
        Task<SyllabusModule> GetSyllabusModule(Guid SyllabusId, Guid ModuleId);
    }
}
