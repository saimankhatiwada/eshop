using Domain.Shared;
using FluentValidation;

namespace Application.Products.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(200);
        RuleFor(C => C.Description).NotEmpty().MaximumLength(2000);
        RuleFor(c => c.Currency).NotEmpty().Must(CheckCurrencyCode);
    }

    private bool CheckCurrencyCode(string currency)
    {
        return !string.IsNullOrEmpty(Currency.FromCode(currency).Code);
    }
}
