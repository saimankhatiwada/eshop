using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Domain.Abstractions;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.CreateProduct;

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            new Name(request.Name),
            new ImageName(request.ImageName),
            new Description(request.Description),
            new Money(request.Amount, Currency.FromCode(request.Currency)),
            Quantity.Create(request.Quantity).Value);
        
        _productRepository.Add(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _storageService.UploadFileAsync(request.ImageName, request.FileContentType, request.FileStream);

        return product.Id.value;
    }
}
