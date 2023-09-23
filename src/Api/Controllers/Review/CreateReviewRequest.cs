namespace Api.Controllers.Review;

public sealed record CreateReviewRequest(
    Guid ProductId,
    Guid UserId,
    int Rating,
    string Comment);
