using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.UserViewModels;
using Domain.Enum.RoleEnum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize()]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Get all users function with no param.
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetAllUsers")]
    public async Task<Pagination<UserViewModel>> GetAllUsers(int pageIndex = 0, int pageSize = 10) => await _userService.GetAllUsers(pageIndex, pageSize);

    /// <summary>
    /// Get user by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetUserById/{UserId}")]
    public async Task<UserViewModel> GetUserById(Guid UserId) => await _userService.GetUserById(UserId);

    /// <summary>
    /// Update user function 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPut("UpdateUser/{UserId}"), Authorize(policy: "AuthUser")]
    public async Task<Response> UpdateUser(Guid UserId, [FromBody] UpdateUserViewModel user) => await _userService.UpdateUser(UserId, user);

    /// <summary>
    /// Get All User by ROLE.
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpGet("GetUserByRole/{role}")]
    public async Task<Pagination<UserViewModel>> GetUsersByRole(Role role, int pageIndex = 0, int pageSize = 10) => await _userService.GetUsersByRole(role,pageIndex,pageSize);

    /// <summary>
    /// Import Users by excel file.
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("UploadFileExcel")]
    public async Task<Response> Import(IFormFile formFile, CancellationToken cancellationToken) => await _userService.UploadFileExcel(formFile, cancellationToken);

    /// <summary>
    /// Change password user
    /// </summary>
    /// <param name="changePassword"></param>
    /// <returns></returns>
    [HttpPut("Change-Password/{UserId}")]
    [Authorize]
    public async Task<Response> ChangePassword(Guid UserId, [FromBody] ChangePasswordViewModel changePassword) => await _userService.ChangePassword(UserId, changePassword);
    

    /// <summary>
    /// Get User by classId
    /// </summary>
    /// <param name="ClassId"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet("GetUsersByClassId/{ClassId}")]
    public async Task<Response> GetUnitByModuleIdAsync(Guid ClassId, int pageIndex = 0, int pageSize = 10)
    {
        return await _userService.GetUserByClassId(ClassId, pageIndex, pageSize);
    }

    /// <summary>
    /// Search Users By Name
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet("SearchUserByName/{name}")]
    public Task<Pagination<UserViewModel>> SearchByName(string name, int pageIndex = 0, int pageSize = 10)
    {
        return _userService.SearchUserByName(name, pageIndex, pageSize);
    }
}

