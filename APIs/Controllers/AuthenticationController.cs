using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Applications.ViewModels.TokenViewModels;
using Microsoft.AspNetCore.Cors;

namespace APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowAll")]
public class AuthenticationController : Controller
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Login")]
    public async Task<Response> Login(UserLoginViewModel userLogin) 
    {
        if(!ModelState.IsValid) return new Response(HttpStatusCode.BadRequest,"worng format");
        return await _userService.Login(userLogin);
    }
    
    [HttpPost("Verify")]
    public async Task<Response> Verify(TokenRequest token)
    {
        return await _userService.VerifyToken(token);
    }

    [HttpPost("RefreshToken")] 
    public async Task<Response> GetRefreshToken(TokenModel oldToken)
    {
        return await _userService.GetRefreshToken(oldToken);
    }
}
