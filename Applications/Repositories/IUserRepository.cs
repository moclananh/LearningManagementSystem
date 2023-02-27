using Applications.Commons;
using Domain.Entities;

namespace Applications.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetUserByEmail(string email);
    Task<Pagination<User>> GetUserByClassId(Guid ClassId, int pageNumber = 0, int pageSize = 10);
}
