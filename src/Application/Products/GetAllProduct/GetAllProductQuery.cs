using Application.Abstractions.Messaging;
using Application.Products.Shared;

namespace Application.Products.GetAllProduct;

public sealed record GetAllProductQuery() : IQuery<IReadOnlyList<ProductResponse>>;
