using Application.Abstractions.Messaging;

namespace Application.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Amount,
    string Currency,
    int Quantity) : ICommand<Guid>;