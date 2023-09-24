namespace Infrastructure.Email;

#pragma warning disable CS8618
public sealed class MailOptions
{
    public string MailHost { get; init; }
    public int MailHostPort { get; init; }
    public string MailHostUsername { get; init; }
    public string MailHostSecretKey { get; init; }
}

#pragma warning restore CS8618