using API.Services.Email;

namespace API.Services.Email.Providers;

public sealed class SendGridEmailSender : IEmailSender
{
    private readonly ILogger<SendGridEmailSender> _logger;
    public SendGridEmailSender(ILogger<SendGridEmailSender> logger) => _logger = logger;

    public Task SendEmailAsync(string to, string subject, string body, CancellationToken ct = default)
    {
        // TODO: Use SendGrid to send email

        _logger.LogInformation("[SendGrid] Sending to {To}. Subject: {Subject}", to, subject);
        return Task.CompletedTask;
    }
}