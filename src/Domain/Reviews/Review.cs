using Domain.Abstractions;
using Domain.Products;
using Domain.Reviews.Events;
using Domain.Users;

namespace Domain.Reviews;

public sealed class Review : Entity<ReviewId>
{
    private Review(
        ReviewId id,
        ProductId productId,
        UserId userId,
        Rating rating,
        Comment comment,
        DateTime createdOnUtc)
        : base(id)
    {
        ProductId = productId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
        CreatedOnUtc = createdOnUtc;
    }

    #pragma warning disable CS8618
    private Review()
    {
    }
    #pragma warning restore CS8618

    public ProductId ProductId { get; private set; }

    public UserId UserId { get; private set; }

    public Rating Rating { get; private set; }

    public Comment Comment { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public static Result<Review> Create(
        ProductId ProductId,
        UserId UserId,
        Rating rating,
        Comment comment,
        DateTime createdOnUtc)
    {

        var review = new Review(
            ReviewId.New(),
            ProductId,
            UserId,
            rating,
            comment,
            createdOnUtc);

        review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id));

        return review;
    }
}