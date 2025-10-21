namespace API.Services.Email;

public sealed class EmailRequest
{
    public string To { get; init; } = "";
    public string Subject { get; init; } = "";
    public string Body { get; init; } = "";

    public bool IsValid(out string? error)
    {
        if (string.IsNullOrWhiteSpace(To))
        {
            error = "Recipient 'to' is required.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(Subject))
        {
            error = "Subject is required.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(Body))
        {
            error = "Body is required.";
            return false;
        }

        error = null;
        return true;
    }
}