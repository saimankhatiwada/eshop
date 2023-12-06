using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Users.Shared;
using Dapper;
using Domain.Abstractions;

namespace Application.Users.GetAllUser;

internal sealed class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, IReadOnlyList<UserResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    private readonly IStorageService _storageService;

    public GetAllUserQueryHandler(
        ISqlConnectionFactory sqlConnectionFactory,
        IStorageService storageService
    )
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _storageService = storageService;
    }
    public async Task<Result<IReadOnlyList<UserResponse>>> Handle(
        GetAllUserQuery request,
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
        """;

        var users = await connection.QueryAsync<UserResponse>(sql, cancellationToken);

        return users
            .Select(user => user with
            {
                ImageName = _storageService.GetPreSignedUrlAsync(user.ImageName).GetAwaiter().GetResult().Value,
            })
            .ToList();
    }
}
