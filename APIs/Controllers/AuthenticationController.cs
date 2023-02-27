using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APIs.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthenticationController : Controller
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<Response> Login(UserLoginViewModel userLogin) 
    {
        if(!ModelState.IsValid) return new Response(HttpStatusCode.BadRequest,"worng format");
        return await _userService.Login(userLogin);
    }

}
