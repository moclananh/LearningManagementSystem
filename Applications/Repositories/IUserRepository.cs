using Applications.Commons;
using Applications.ViewModels.UserViewModels;
using Domain.Entities;
using Domain.Enum.RoleEnum;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;

namespace Applications.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetUserByEmail(string email);
    Task<Pagination<User>> GetUserByClassId(Guid ClassId, int pageNumber = 0, int pageSize = 10);
    Task<Pagination<User>> GetUsersByRole(Role role, int pageNumber = 0, int pageSize = 10);
    Task<Pagination<User>> SearchUserByName(string name, int pageNumber = 0, int pageSize = 10);
    Task<Pagination<User>> FilterUser(FilterUserRequest filterUserRequest,int pageNumber = 0, int pageSize = 10);
    Task<User?> GetUserByPasswordResetToken(string token);
}
