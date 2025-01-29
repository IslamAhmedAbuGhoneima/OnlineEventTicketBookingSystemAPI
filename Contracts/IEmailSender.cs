namespace Contracts;

public interface IEmailSender
{
    Task SendEmailAsync(string mail, string subject, string message);
}
