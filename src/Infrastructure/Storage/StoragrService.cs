using Amazon.S3;
using Amazon.S3.Model;
using Application.Abstractions.Clock;
using Application.Abstractions.Storage;
using Domain.Abstractions;
using Microsoft.Extensions.Options;

namespace Infrastructure.Storage;

internal sealed class StoragrService : IStorageService
{
    private readonly IAmazonS3 _amazonS3;
    private readonly S3BucketOptions _s3BucketOptions;
    private readonly IDateTimeProvider _dateTimeProvider;

    public StoragrService(
        IAmazonS3 amazonS3,
        IOptions<S3BucketOptions> s3BucketOptions,
        IDateTimeProvider dateTimeProvider
    )
    {
        _amazonS3 = amazonS3;
        _s3BucketOptions = s3BucketOptions.Value;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<string>> GetPreSignedUrlAsync(string FileName)
    {
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, _s3BucketOptions.BucketName);
        if (!bucketExists) return Result.Failure<string>(StorageErrors.DoesNotExists);
        var urlRequest = new GetPreSignedUrlRequest()
        {
            BucketName = _s3BucketOptions.BucketName,
            Key = FileName,
            Expires = _dateTimeProvider.UtcNow.AddMinutes(_s3BucketOptions.ExpiresOn)
        };

        return _amazonS3.GetPreSignedURL(urlRequest);
    }

    public async Task<Result<string>> UploadFileAsync(
        string FileName, 
        string ContentType, 
        Stream FileStream)
    {
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, _s3BucketOptions.BucketName);
        if (!bucketExists) return Result.Failure<string>(StorageErrors.DoesNotExists);
        var request = new PutObjectRequest()
        {
            BucketName = _s3BucketOptions.BucketName,
            Key = FileName,
            InputStream = FileStream
        };
        request.Metadata.Add("Content-Type", ContentType);

        var result = await _amazonS3.PutObjectAsync(request);

        return result.HttpStatusCode.ToString();
    }
}
