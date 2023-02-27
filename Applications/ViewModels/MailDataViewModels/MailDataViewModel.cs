
namespace Applications.ViewModels.MailDataViewModels;

public class MailDataViewModel
{
    // Receiver
    public List<string> To { get; } 

    // Content
    public string Subject { get; }

    public string? Body { get; }

    public MailDataViewModel(List<string> to, string subject, string? body = null)
    {
        // Receiver
        To = to;

        // Content
        Subject = subject;
        Body = body;
    }
}
