using Domain.Abstractions;

namespace Application.Abstractions.Storage;

public interface IStorageService
{
    Task<Result<string>> UploadFileAsync(string FileName, string ContentType, Stream FileStream);
    Task<Result<string>> GetPreSignedUrlAsync(string FileName);
    Task<Result> DeleteFileAsync(string FileName);
}
