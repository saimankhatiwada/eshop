using Domain.Shared;
using FluentValidation;

namespace Application.Products.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(c => c.ProductId)
            .NotEmpty()
            .WithMessage("An identifier can never be empty");

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.")
            .MaximumLength(200)
            .WithMessage("Name cannont be longer than 200 character.");

        RuleFor(C => C.Description)
            .NotEmpty()
            .WithMessage("Name cannot be empty.")
            .MaximumLength(2000)
            .WithMessage("Description cannont be longer than 2000 character.");

        RuleFor(c => c.Amount)
            .NotNull()
            .WithMessage("Amount is required")
            .GreaterThanOrEqualTo(1)
            .WithMessage("Amount should be equal or greater than 1.");

        RuleFor(c => c.Quantity)
            .NotNull()
            .WithMessage("Quantity is required")
            .GreaterThanOrEqualTo(1)
            .WithMessage("Quantity should be equal or greater than 1.");

        RuleFor(c => c.Currency)
            .NotEmpty()
            .WithMessage("Currency Cannot be empty.")
            .Must(CheckCurrencyCode)
            .WithMessage("Invalid currency.");
    }

    private bool CheckCurrencyCode(string currency)
    {
        return !string.IsNullOrEmpty(Currency.CheckCode(currency).Code);
    }
}
