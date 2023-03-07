using Applications.Commons;
using Domain.Entities;
using Domain.Enum.RoleEnum;

namespace Applications.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetUserByEmail(string email);
    Task<Pagination<User>> GetUserByClassId(Guid ClassId, int pageNumber = 0, int pageSize = 10);
    Task<Pagination<User>> GetUsersByRole(Role role, int pageNumber = 0, int pageSize = 10);
    Task<Pagination<User>> SearchUserByName(string name, int pageNumber = 0, int pageSize = 10);
}
