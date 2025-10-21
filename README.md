# 🔑 Keyed Services in .NET – Email Providers Example

This project demonstrates how to use **Keyed Services** in .NET 8 to cleanly handle multiple implementations of the same interface — in this case, different **email providers**.

---

## 🚀 Overview

The app registers several `IEmailSender` implementations (SMTP, SendGrid, and Mailgun) under unique keys.  
It shows both **static** and **dynamic** resolution:

- **Static**: when the provider is known at compile time using `[FromKeyedServices]`
- **Dynamic**: when the provider is chosen at runtime via a factory delegate  

This pattern works well for multi-tenant apps, feature flags, or routing logic.

---

## ⚙️ How It Works

Each provider is registered with a key:

```csharp
builder.Services.AddKeyedSingleton<IEmailSender, SmtpEmailSender>(EmailProviderOption.Smtp);
builder.Services.AddKeyedSingleton<IEmailSender, SendGridEmailSender>(EmailProviderOption.SendGrid);
builder.Services.AddKeyedSingleton<IEmailSender, MailgunEmailSender>(EmailProviderOption.MailgunEmail);
```

A delegate factory resolves them dynamically:

```csharp
builder.Services.AddSingleton<Func<EmailProviderOption, IEmailSender>>(sp =>
    key => sp.GetRequiredKeyedService<IEmailSender>(key));
```

---

## 🌐 Endpoints

### `/emails/smtp`
Uses **static resolution** — always sends with the SMTP provider.

### `/emails`
Uses **dynamic resolution** — selects the provider from the `provider` query parameter (defaults to SMTP).

**Example request:**
```
POST /emails?provider=SendGrid
{
  "to": "user@example.com",
  "subject": "Hello",
  "body": "This is a test email."
}
```

**Response:**
```json
{
  "sentWith": "SendGrid",
  "to": "user@example.com"
}
```

---

## 🧩 When to Use Keyed Services

- Multiple strategies under one interface (e.g., different email providers)  
- Tenant-specific implementations  
- Routing or load-balancing logic  
- Feature toggles or mode-based behavior  

---

## 🧪 Run It

```bash
git clone https://github.com/<your-username>/<repo-name>.git
cd <repo-name>
dotnet run
```

Then open:
```
https://localhost:5001/swagger
```
