using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Products.Shared;
using Dapper;
using Domain.Abstractions;

namespace Application.Products.GetAllProduct;

internal sealed class GetAllProductQueryHandler : IQueryHandler<GetAllProductQuery, IReadOnlyList<ProductResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IStorageService _storageService;

    public GetAllProductQueryHandler(
        ISqlConnectionFactory sqlConnectionFactory,
        IStorageService storageService)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _storageService = storageService;
    }
    public async Task<Result<IReadOnlyList<ProductResponse>>> Handle(
        GetAllProductQuery request, 
        CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
        SELECT
            id AS Id,
            name AS Name,
            image_name AS ImageName,
            description AS Description,
            money_amount AS Amount,
            money_currency AS Currency,
            quantity AS Quantity
        FROM products
        WHERE
            (@greaterThan IS NULL AND @lessThan IS NULL) 
            OR
            (CASE
                WHEN @greaterThan IS NOT NULL AND @lessThan IS NOT NULL THEN money_amount > @greaterThan AND money_amount < @lessThan
                WHEN @lessThan IS NULL THEN money_amount > @greaterThan
                WHEN @greaterThan IS NULL THEN money_amount < @lessThan
                ELSE 1=1
            END)
        ORDER BY
            CASE
                WHEN @sortColumn = 'amount' THEN
                    CASE
                        WHEN @sortOrder = 'asc' THEN money_amount
                        WHEN @sortOrder = 'desc' THEN money_amount * -1
                        ELSE money_amount
                    END
                WHEN @sortColumn = 'quantity' THEN
                    CASE
                        WHEN @sortOrder = 'asc' THEN quantity
                        WHEN @sortOrder = 'desc' THEN quantity * -1
                        ELSE quantity
                    END
                ELSE
                    CASE
                        WHEN @sortOrder = 'desc' THEN money_amount * -1
                        ELSE money_amount
                    END
            END
        ";

        var parameters = new 
        {
            request.greaterThan,
            request.lessThan,
            request.sortColumn,
            request.sortOrder
        };

        var products = await connection.QueryAsync<ProductResponse>(sql, parameters);

        return products
            .Select(product => product with
            {
                ImageName = _storageService.GetPreSignedUrlAsync(product.ImageName).GetAwaiter().GetResult().Value,
            })
            .ToList();
    }
}
