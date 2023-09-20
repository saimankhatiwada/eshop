using FluentValidation;

namespace Application.Products.DeleteProduct;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(c => c.ProductId)
            .NotEmpty()
            .WithMessage("An identifier can never be empty");
    }
}
