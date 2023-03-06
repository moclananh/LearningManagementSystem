using Applications.Repositories;
using Domain.EntityRelationship;

namespace Application.Repositories
{
    public interface IClassUserRepository : IGenericRepository<ClassUser>
    {
        Task UploadClassUserListAsync(List<ClassUser> classUser);
        Task<ClassUser> GetClassUser(Guid ClassId, Guid UserId);
        Task<List<ClassUser>> GetClassUserListByClassId(Guid ClassId);
    }
}
