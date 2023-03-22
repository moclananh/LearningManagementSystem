using Applications.ViewModels.MailDataViewModels;
using Applications.ViewModels.Response;
using Domain.Entities;

namespace Applications.Interfaces.EmailServicesInterface;

public interface IMailService
{
    Task<bool> SendAsync(MailDataViewModel mailData, CancellationToken ct);
    //Task SendEmailAsync(MailRequest mailData);
    
    Task<string> GetEmailTemplateForgotPassword(string nameTemplate,string email);
    Task GetEmailAbsent(User user,Class Class);
    Task SendAbsentEmail();
    //Task<bool> GetEmailAbsentTest(User user, Class Class);
    //Task<Response> SendAbsentEmailTest();
}
