namespace Infrastructure.Storage;

#pragma warning disable CS8618
public sealed class S3SecurityOptions
{
    public string AccessKey { get; init; }

    public string SecretKey { get; init; }
}

#pragma warning restore CS8618