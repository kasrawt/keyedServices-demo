﻿namespace API.Services.Email;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken ct = default);
}