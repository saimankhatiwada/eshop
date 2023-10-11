using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Products.Shared;
using Dapper;
using Domain.Abstractions;

namespace Application.Products.GetProduct;

internal sealed class GetProductQueryHandler : IQueryHandler<GetProductQuery, ProductResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IStorageService _storageService;

    public GetProductQueryHandler(
        ISqlConnectionFactory sqlConnectionFactory,
        IStorageService storageService)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _storageService = storageService;
    }
    public async Task<Result<ProductResponse>> Handle(
        GetProductQuery request, 
        CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
        SELECT
            id AS Id,
            name AS Name,
            image_name AS ImageName,
            description AS Description,
            money_amount AS Amount,
            money_currency AS Currency,
            quantity AS Quantity
        FROM products
        WHERE Id = @ProductId
        """;

        var product = await connection.QueryFirstOrDefaultAsync<ProductResponse>(
            sql,
            new
            {
                request.ProductId
            });

        return product with 
            {
                ImageName = _storageService.GetPreSignedUrlAsync(product.ImageName).GetAwaiter().GetResult().Value
            };
    }
}
