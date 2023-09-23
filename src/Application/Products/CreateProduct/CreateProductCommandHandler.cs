using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.CreateProduct;

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            new Name(request.Name),
            new Description(request.Description),
            new Money(request.Amount, Currency.FromCode(request.Currency)),
            Quantity.Create(request.Quantity).Value);
        
        _productRepository.Add(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id.value;
    }
}
