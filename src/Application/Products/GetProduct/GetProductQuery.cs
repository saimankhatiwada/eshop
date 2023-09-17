using Application.Abstractions.Messaging;
using Application.Products.Shared;

namespace Application.Products.GetProduct;

public sealed record GetProductQuery(Guid ProductId) : IQuery<ProductResponse>;
