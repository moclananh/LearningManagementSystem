using Applications.Repositories;
using Domain.EntityRelationship;

namespace Application.Repositories
{
    public interface IClassUserRepository : IGenericRepository<ClassUser>
    {
        Task UploadClassUserListAsync(List<ClassUser> classUser);
    }
}
