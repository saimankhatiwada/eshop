using Domain.Abstractions;
using Domain.Reviews.Events;

namespace Domain.Reviews;

public sealed class Review : Entity
{
    private Review(
        Guid id,
        Guid productId,
        Guid userId,
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

    public Guid ProductId { get; private set; }

    public Guid UserId { get; private set; }

    public Rating Rating { get; private set; }

    public Comment Comment { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public static Result<Review> Create(
        Guid ProductId,
        Guid UserId,
        Rating rating,
        Comment comment,
        DateTime createdOnUtc)
    {

        var review = new Review(
            Guid.NewGuid(),
            ProductId,
            UserId,
            rating,
            comment,
            createdOnUtc);

        review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id));

        return review;
    }
}