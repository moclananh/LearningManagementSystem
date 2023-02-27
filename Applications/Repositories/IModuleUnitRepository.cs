using Domain.EntityRelationship;


namespace Applications.Repositories
{
    public interface IModuleUnitRepository : IGenericRepository<ModuleUnit>
    {
        Task<ModuleUnit> GetModuleUnit(Guid ModuleId, Guid UnitId);
    }
}
