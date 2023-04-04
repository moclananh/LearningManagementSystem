using Applications.Interfaces.EmailServicesInterface;
using Applications.ViewModels.MailDataViewModels;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize(policy: "AuthUser")]
[EnableCors("AllowAll")]
public class MailController : Controller
{
    private readonly IMailService _mailService;
	public MailController(IMailService mailService)
	{
		_mailService = mailService;
	}
	
	[HttpPost("forgotPasswordByEmail")]
    [AllowAnonymous]
    public async Task<IActionResult> forgotPasswordByEmail(string email)
	{
		string body = await _mailService.GetEmailTemplateForgotPassword("forgotPassword", email);
		if (body == null) return StatusCode(StatusCodes.Status400BadRequest, "Email does not exist in the system!!");

        MailDataViewModel mailData = new MailDataViewModel(
			new List<string> { email },
			"WELCOME TO LMS FAKE", 
			 body
			);

		bool sendResult = await _mailService.SendAsync(mailData, new CancellationToken());
		if(sendResult)
            return StatusCode(StatusCodes.Status200OK, " success!");
        return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
    }

 //   [HttpGet("DemoCronjobFunctionUsingAPI")]
	//public async Task<Response> Test() => await _mailService.SendAbsentEmailTest();

    /*
	[HttpPost("SendEmail")]
	public async Task<IActionResult> SendEmail([FromForm]MailRequest mailRequest)
	{
		try
		{
			await _mailService.SendEmailAsync(mailRequest);
			return Ok();
		}catch(Exception ex)
		{
			throw;
		}
	}
	*/

}
