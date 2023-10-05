using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Products.Shared;
using Dapper;
using Domain.Abstractions;

namespace Application.Products.GetAllProduct;

internal sealed class GetAllProductQueryHandler : IQueryHandler<GetAllProductQuery, IReadOnlyList<ProductResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAllProductQueryHandler(
        ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    public async Task<Result<IReadOnlyList<ProductResponse>>> Handle(
        GetAllProductQuery request, 
        CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
        SELECT
            id AS Id,
            name AS Name,
            image_name AS ImagrName,
            description AS Description,
            money_amount AS Amount,
            money_currency AS Currency,
            quantity AS Quantity
        FROM products
        """;

        var product = await connection.QueryAsync<ProductResponse>(sql, cancellationToken);

        var products = product.ToList();

        return products;
    }
}
