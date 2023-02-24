using Applications.ViewModels.Response;
using Applications.ViewModels.UserViewModels;
using Domain.Enum.RoleEnum;
using Microsoft.AspNetCore.Http;

namespace Applications.Interfaces;

public interface IUserService
{
    Task<List<UserViewModel>> GetAllUsers();
    Task<UserViewModel> GetUserById(Guid id);
    Task<Response> Login(UserLoginViewModel userLoginViewModel);
    Task<Response> UpdateUser(Guid id, UpdateUserViewModel updateUserViewModel);
    Task<List<UserViewModel>> GetUsersByRole(Role role);
    Task<Response> UploadFileExcel(IFormFile formFile, CancellationToken cancellationToken);

}
