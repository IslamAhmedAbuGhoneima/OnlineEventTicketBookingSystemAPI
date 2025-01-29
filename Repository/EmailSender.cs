using Contracts;
using Entities.ConfigurationModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Repository;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> config)
    {
        _emailSettings = config.Value;   
    }

    public async Task SendEmailAsync(string mail, string subject, string message)
    {

        string host = _emailSettings.SmtpServer;
        int port = _emailSettings.Port;
        string username = _emailSettings.Username;
        string password = _emailSettings.Password;

        using (var client = new SmtpClient(host, port)) 
        {
            client.Credentials = new NetworkCredential(username, password);
            client.EnableSsl = true;


            var mailMessage = new MailMessage(username, mail, subject, message);
            
            await client.SendMailAsync(mailMessage);
        }
    }
}
