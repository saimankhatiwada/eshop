using Application.Abstractions.Messaging;
using Application.Products.Shared;

namespace Application.Products.GetAllProduct;

public sealed record GetAllProductQuery(double? greaterThan, double? lessThan, string? sortColumn, string? sortOrder) : IQuery<IReadOnlyList<ProductResponse>>;
