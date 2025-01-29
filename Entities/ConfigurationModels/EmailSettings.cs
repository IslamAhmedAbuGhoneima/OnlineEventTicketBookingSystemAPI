namespace Entities.ConfigurationModels;

public class EmailSettings
{
    public string SmtpServer { get; set; } = null!;

    public int Port { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

}
