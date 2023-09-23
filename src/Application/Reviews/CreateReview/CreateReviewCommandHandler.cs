using Application.Abstractions.Clock;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Products;
using Domain.Reviews;
using Domain.Users;

namespace Application.Reviews.CreateReview;

internal sealed class CreateReviewCommandHandler : ICommandHandler<CreateReviewCommand, Guid>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReviewCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IUserRepository userRepository,
        IProductRepository productRepository,
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork)
    {
        _dateTimeProvider = dateTimeProvider;
        _userRepository = userRepository;
        _productRepository = productRepository;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateReviewCommand request, 
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        var product = await _productRepository.GetByIdAsync(new ProductId(request.ProductId), cancellationToken);

        if (product is null)
        {
            return Result.Failure<Guid>(ProductErrors.NotFound);
        }

        var review = Review.Create(
            product.Id,
            user.Id,
            Rating.Create(request.Rating).Value,
            new Comment(request.Comment),
            _dateTimeProvider.UtcNow);

        _reviewRepository.Add(review.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return review.Value.Id.value;
    }
}
