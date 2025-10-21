using API.Services.Email;

namespace API.Services.Email.Providers;

public sealed class SmtpEmailSender : IEmailSender
{
    private readonly ILogger<SmtpEmailSender> _logger;
    public SmtpEmailSender(ILogger<SmtpEmailSender> logger) => _logger = logger;

    public Task SendEmailAsync(string to, string subject, string body, CancellationToken ct = default)
    {
        // TODO: Use SmtpClient to send email

        _logger.LogInformation("[SMTP] Sending to {To}. Subject: {Subject}", to, subject);
        return Task.CompletedTask;
    }
}