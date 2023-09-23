using FluentValidation;

namespace Application.Reviews.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(c => c.ProductId)
            .NotEmpty()
            .WithMessage("An identifier can never be empty");

        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("An identifier can never be empty");

        RuleFor(c => c.Rating)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Rating cannot be less than 1.")
            .LessThanOrEqualTo(5)
            .WithMessage("Rating cannot be grater than 5.");

        RuleFor(c => c.Comment)
            .NotEmpty()
            .WithMessage("Comment cannot be empty");
    }
}
