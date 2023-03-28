using Applications.Interfaces.EmailServicesInterface;
using Applications.Utils;
using Applications.ViewModels.MailDataViewModels;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using System.Text;
using RazorEngineCore;
using Applications.Interfaces;
using Domain.Entities;

namespace Applications.Services.EmailServices;

public class MailService : IMailService
{
    private readonly MailSetting _setting;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    public MailService(IOptions<MailSetting> mailSetting, IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        _setting = mailSetting.Value;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<string> GetEmailTemplateForgotPassword(string nameTemplate, string email)
    {
        string mailTemplate = LoadTemplate(nameTemplate);
        var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
        if (user == null) return null;

        // check 
        string code = StringUtils.RandomString();
        user.PasswordResetToken = code;
        user.ResetTokenExpires = DateTime.Now.AddMinutes(15);
        await _unitOfWork.SaveChangeAsync();
        EmailTemplateModel emailTemplateModel = new EmailTemplateModel
        {
            FirstName = user.firstName,
            LastName = user.lastName,
            Email = user.Email,
            URL = $"http://localhost:4200/authentication/change-password?token={code}"
        };

        IRazorEngine razorEngine = new RazorEngine();
        IRazorEngineCompiledTemplate modifiledMailTemplate = razorEngine.Compile(mailTemplate);
        return modifiledMailTemplate.Run(emailTemplateModel);
    }

    public async Task GetEmailAbsent(User user,Class Class)
    {
        string mailTemplate = LoadTemplate("Cronjob");
        // check 
        EmailTemplateModel emailTemplateModel = new EmailTemplateModel
        {
            FirstName = user.firstName,
            LastName = user.lastName,
            Email = user.Email,
            Date = DateTime.Today.ToString(),
            ClassCode = Class.ClassCode
        };
        IRazorEngine razorEngine = new RazorEngine();
        IRazorEngineCompiledTemplate modifiledMailTemplate = razorEngine.Compile(mailTemplate);
        MailDataViewModel mailData = new MailDataViewModel(
            new List<string> { user.Email },
            "WELCOME TO LMS FAKE",
            modifiledMailTemplate.Run(emailTemplateModel)
            );
            bool sendResult = await SendAsync(mailData, new CancellationToken());
    }

    //public async Task<bool> GetEmailAbsentTest(User user, Class Class)
    //{
    //    string mailTemplate = LoadTemplate("Cronjob");
    //    // check 
    //    EmailTemplateModel emailTemplateModel = new EmailTemplateModel
    //    {
    //        FirstName = user.firstName,
    //        LastName = user.lastName,
    //        Email = user.Email,
    //        Date = DateTime.Today.ToString(),
    //        ClassCode = Class.ClassCode
    //    };
    //    IRazorEngine razorEngine = new RazorEngine();
    //    IRazorEngineCompiledTemplate modifiledMailTemplate = razorEngine.Compile(mailTemplate);
    //    MailDataViewModel mailData = new MailDataViewModel(
    //        new List<string> { user.Email },
    //        "WELCOME TO LMS FAKE",
    //        modifiledMailTemplate.Run(emailTemplateModel)
    //        );
    //    bool sendResult = await SendAsync(mailData, new CancellationToken());
    //    return sendResult;
    //}
    public async Task SendAbsentEmail()
    {
        List<Attendance> ListAbsent = await _unitOfWork.AttendanceRepository.GetAbsentId();
        foreach (var item in ListAbsent)
        {
            var User = await _unitOfWork.UserRepository.GetByIdAsync(item.UserId);
            var Class = await _unitOfWork.ClassRepository.GetByIdAsync(item.ClassId);
            await GetEmailAbsent(User, Class);
        }
    }

    //public async Task<Response> SendAbsentEmailTest()
    //{
    //    List<Attendance> ListAbsent = await _unitOfWork.AttendanceRepository.GetAbsentId();
    //    bool test = false;
    //    foreach (var item in ListAbsent)
    //    {
    //        var User = await _unitOfWork.UserRepository.GetByIdAsync(item.UserId);
    //        var Class = await _unitOfWork.ClassRepository.GetByIdAsync(item.ClassId);
    //        var a = await GetEmailAbsentTest(User, Class);
    //        if (a)
    //        {
    //            test = true;
    //        }
    //    }
    //    if (test)
    //    {
    //        return new Response(HttpStatusCode.OK, "Success");
    //    }
    //    return new Response(HttpStatusCode.BadRequest, "Failed");
    //}

    private string LoadTemplate(string nameTemplate)
    {
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Templates", $"{nameTemplate}.cshtml");
        using FileStream fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using StreamReader sr = new StreamReader(fileStream, Encoding.Default);

        string mailTemplate = sr.ReadToEnd();
        sr.Close();
        return mailTemplate;
    }

    public async Task<bool> SendAsync(MailDataViewModel mailData, CancellationToken ct)
    {
        try
        {
            // Initialize a new instance MimeMessage 
            var mail = new MimeMessage();

            //sender
            mail.Sender = MailboxAddress.Parse(_setting.From);

            //Receiver
            foreach (string mailAddress in mailData.To)
            {
                mail.To.Add(MailboxAddress.Parse(mailAddress));
            }

            //Add content to MineMessage
            var body = new BodyBuilder();
            mail.Subject = mailData.Subject;
            body.HtmlBody = mailData.Body;
            mail.Body = body.ToMessageBody();

            //Send Email
            using var smtp = new SmtpClient();
            if (_setting.UseSSL) await smtp.ConnectAsync(_setting.Host, _setting.Port, SecureSocketOptions.SslOnConnect, ct);
            if (_setting.UseStartTls) await smtp.ConnectAsync(_setting.Host, _setting.Port, SecureSocketOptions.StartTls, ct);

            await smtp.AuthenticateAsync(_setting.UserName, _setting.Password, ct);
            await smtp.SendAsync(mail, ct);
            await smtp.DisconnectAsync(true, ct);

            return true;

        }
        catch (Exception ex)
        {
            return false;
        }
    }

    /*
public async Task SendEmailAsync(MailRequest mailData)
{
   var email = new MimeMessage();
   email.Sender = MailboxAddress.Parse(_setting.From);
   email.To.Add(MailboxAddress.Parse(mailData.ToEmail));
   email.Subject = mailData.Subject;
   var builder = new BodyBuilder();
   if(mailData.Attachments != null)
   {
       byte[] fileBytes;
       foreach(var file in mailData.Attachments)
       {
           if(file.Length > 0)
           {
               using(var ms = new MemoryStream())
               {
                   file.CopyTo(ms);
                   fileBytes = ms.ToArray();
               }
               builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
           }
       }
   }
   builder.HtmlBody = mailData.Body;
   email.Body = builder.ToMessageBody();
   using var smtp = new SmtpClient();
   smtp.Connect(_setting.Host, _setting.Port,SecureSocketOptions.StartTls);
   smtp.Authenticate(_setting.UserName, _setting.Password);
   await smtp.SendAsync(email);
   smtp.Disconnect(true);
}
*/

}
