namespace Infrastructure.Storage;

#pragma warning disable CS8618
public sealed class S3BucketOptions
{
    public string BucketName { get; init; }
    public int ExpiresOn { get; init; }
    public string Products { get; init; }
    public string Users { get; init; }
}

#pragma warning restore CS8618