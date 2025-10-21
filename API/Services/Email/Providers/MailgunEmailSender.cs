using API.Services.Email;

namespace API.Services.Email.Providers;

public sealed class MailgunEmailSender : IEmailSender
{
    private readonly ILogger<MailgunEmailSender> _logger;
    public MailgunEmailSender(ILogger<MailgunEmailSender> logger) => _logger = logger;

    public Task SendEmailAsync(string to, string subject, string body, CancellationToken ct = default)
    {
        // TODO: Use Mailgun to send email

        _logger.LogInformation("[Mailgun] Sending to {To}. Subject: {Subject}", to, subject);
        return Task.CompletedTask;
    }
}