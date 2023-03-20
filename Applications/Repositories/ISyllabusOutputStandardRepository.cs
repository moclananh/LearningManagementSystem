using Domain.EntityRelationship;
namespace Applications.Repositories
{
    public interface ISyllabusOutputStandardRepository : IGenericRepository<SyllabusOutputStandard>
    {
        Task<SyllabusOutputStandard> GetSyllabusOutputStandard(Guid SyllabusId, Guid OutputStandardId);
    }
}

