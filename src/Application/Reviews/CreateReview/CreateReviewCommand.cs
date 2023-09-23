using Application.Abstractions.Messaging;

namespace Application.Reviews.CreateReview;

public record CreateReviewCommand(
    Guid ProductId,
    Guid UserId,
    int Rating,
    string Comment) : ICommand<Guid>;
