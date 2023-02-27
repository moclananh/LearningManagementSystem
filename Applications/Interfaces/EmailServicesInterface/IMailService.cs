using Applications.ViewModels.MailDataViewModels;

namespace Applications.Interfaces.EmailServicesInterface;

public interface IMailService
{
    Task<bool> SendAsync(MailDataViewModel mailData, CancellationToken ct);
    //Task SendEmailAsync(MailRequest mailData);
    
    Task<string> GetEmailTemplate(string nameTemplate,string email);
}
