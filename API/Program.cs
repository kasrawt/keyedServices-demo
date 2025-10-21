using API.Services.Email;
using API.Services.Email.Providers;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Register Keyed Services
builder.Services.AddKeyedSingleton<IEmailSender, SmtpEmailSender>(EmailProviderOption.Smtp);
builder.Services.AddKeyedSingleton<IEmailSender, SendGridEmailSender>(EmailProviderOption.SendGrid);
builder.Services.AddKeyedSingleton<IEmailSender, MailgunEmailSender>(EmailProviderOption.MailgunEmail);

// Add a delegate factory for dynamic resolution
builder.Services.AddSingleton<Func<EmailProviderOption, IEmailSender>>(sp =>
    key => sp.GetRequiredKeyedService<IEmailSender>(key));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// STATIC resolution: compile-time known provider via [FromKeyedServices]
// Example: Force SMTP for a health-check or ops-only route.
app.MapPost("/emails/smtp", async (
    [FromKeyedServices(EmailProviderOption.Smtp)] IEmailSender sender,
    [FromBody] EmailRequest dto,
    CancellationToken ct) =>
{
    if (!dto.IsValid(out var err))
        return Results.BadRequest(err);

    await sender.SendEmailAsync(dto.To, dto.Subject, dto.Body, ct);
    return Results.Ok(new { sentWith = EmailProviderOption.Smtp.ToString(), to = dto.To });
})
.WithName("SendEmailUsingSmtp")
.WithOpenApi();

// DYNAMIC resolution: runtime-selected provider via delegate factory
// Example: Pick provider from querystring, header, tenant, or feature flag.
app.MapPost("/emails", async (
    [FromServices] Func<EmailProviderOption, IEmailSender> emailProvider,
    [FromBody] EmailRequest dto,
    [FromQuery] EmailProviderOption? provider,
    CancellationToken ct) =>
{
    if (!dto.IsValid(out var err))
        return Results.BadRequest(err);

    var chosenProviderOption = provider ?? EmailProviderOption.Smtp;
    var sender = emailProvider(chosenProviderOption);

    await sender.SendEmailAsync(dto.To, dto.Subject, dto.Body, ct);
    return Results.Ok(new { sentWith = chosenProviderOption.ToString(), to = dto.To });
})
.WithName("SendEmail")
.WithOpenApi();

app.Run();