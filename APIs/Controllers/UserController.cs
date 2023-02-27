using Application.ViewModels.UnitViewModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusViewModels;
using Applications.ViewModels.UserViewModels;
using Domain.Enum;
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
    //private readonly ISendGridClient _sendGridClient;
    private readonly IConfiguration _configuration;

    public UserController(IUserService userService,IConfiguration configuration)
    {

        _userService = userService;
        _configuration = configuration;
    }

    /// <summary>
    /// Get all users function with no param.
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetAllUsers")]
    public async Task<List<UserViewModel>> GetAllUsers() => await _userService.GetAllUsers();

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
    [HttpPut("UpdateUser/{UserId}")]
    public async Task<Response> UpdateUser(Guid UserId, [FromBody] UpdateUserViewModel user) => await _userService.UpdateUser(UserId, user);

    /// <summary>
    /// Get All User by ROLE.
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpGet("GetUserByRole/{role}")]
    public async Task<List<UserViewModel>> GetUsersByRole(Role role) => await _userService.GetUsersByRole(role);

    /// <summary>
    /// Import Users by excel file.
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("UploadFileExcel")]
    public async Task<Response> Import(IFormFile formFile, CancellationToken cancellationToken) => await _userService.UploadFileExcel(formFile, cancellationToken);

    /*
    [HttpGet("sent-text-email")]
    public async Task<IActionResult> SendPlainTextEmail(string toEmail)
    {
        string formEmail = _configuration.GetSection("SendGridEmailSettings").GetValue<string>("FromEmail");
        string fromName =  _configuration.GetSection("SendGridEmailSettings").GetValue<string>("FromName");

        var msg = new SendGridMessage()
        {
            From = new EmailAddress(formEmail, fromName),
            Subject = "Plain Text Email",
            PlainTextContent = "Hello, WellCome!!!"
        };
        
        msg.AddTo(toEmail);
        var response = await _sendGridClient.SendEmailAsync(msg);
        string message = response.IsSuccessStatusCode ? $"success! + {toEmail}" : "failed!";
        return Ok(message);
    }
    */

    [HttpGet("GetUsersByClassId/{ClassId}")]
    public async Task<Pagination<UserViewModel>> GetUnitByModuleIdAsync(Guid ClassId, int pageIndex = 0, int pageSize = 10)
    {
        return await _userService.GetUserByClassId(ClassId, pageIndex, pageSize);
    }
}

