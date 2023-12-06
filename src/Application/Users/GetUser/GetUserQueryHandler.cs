using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Users.Shared;
using Dapper;
using Domain.Abstractions;

namespace Application.Users.GetUser;

internal sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IStorageService _storageService;

    public GetUserQueryHandler(
        ISqlConnectionFactory sqlConnectionFactory,
        IStorageService storageService
    )
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _storageService = storageService;
    }
    public async Task<Result<UserResponse>> Handle(
        GetUserQuery request,
        CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
        SELECT
            id As Id,
            first_name As FirstName,
            last_name As LastName,
            email As Email,
            image_name As ImageName
        FROM Users
        WHERE Id = @UserId
        """;

        var user = await connection.QueryFirstOrDefaultAsync<UserResponse>(
            sql,
            new
            {
                request.UserId
            });

#pragma warning disable CS8602 
        return user with
        {
            ImageName = _storageService.GetPreSignedUrlAsync(user.ImageName).GetAwaiter().GetResult().Value
        };
#pragma warning restore CS8602 
    }
}
