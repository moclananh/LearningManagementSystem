using Applications.Commons;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusViewModels;
using Applications.ViewModels.UserViewModels;
using Domain.Enum.RoleEnum;
using Microsoft.AspNetCore.Http;

namespace Applications.Interfaces;

public interface IUserService
{
    Task<Pagination<UserViewModel>> GetAllUsers(int pageIndex = 0, int pageSize = 10);
    Task<UserViewModel> GetUserById(Guid id);
    Task<Response> Login(UserLoginViewModel userLoginViewModel);
    Task<Response> UpdateUser(Guid id, UpdateUserViewModel updateUserViewModel);
    Task<Pagination<UserViewModel>> GetUsersByRole(Role role, int pageIndex = 0, int pageSize = 10);
    Task<Response> UploadFileExcel(IFormFile formFile, CancellationToken cancellationToken);
    Task<Response> GetUserByClassId(Guid ClassId, int pageIndex = 0, int pageSize = 10);
    Task<Response> ChangePassword(Guid id, ChangePasswordViewModel changePassword);
    Task<Pagination<UserViewModel>> SearchUserByName(string name, int pageIndex = 0, int pageSize = 10);
}
