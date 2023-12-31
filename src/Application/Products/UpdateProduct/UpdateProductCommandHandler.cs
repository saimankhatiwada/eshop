using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Domain.Abstractions;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(
        UpdateProductCommand request, 
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(new ProductId(request.ProductId), cancellationToken);

        if (product is null)
        {
            return Result.Failure(ProductErrors.NotFound);
        }

        await _storageService.DeleteFileAsync(product.ImageName.Value);

        var result = product?.Update(
            new Name(request.Name),
            new ImageName(request.ImageName),
            new Description(request.Description),
            new Money(request.Amount, Currency.FromCode(request.Currency)),
            Quantity.Create(request.Quantity).Value);

        if (result!.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _storageService.UploadFileAsync(request.ImageName, request.FileContentType, request.FileStream);

        return Result.Success();
    }
}
