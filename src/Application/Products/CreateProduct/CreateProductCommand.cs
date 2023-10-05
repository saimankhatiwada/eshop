using Application.Abstractions.Messaging;

namespace Application.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    string ImageName,
    string Description,
    decimal Amount,
    string Currency,
    int Quantity) : ICommand<Guid>;
