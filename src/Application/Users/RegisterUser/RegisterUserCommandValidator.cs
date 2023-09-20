using FluentValidation;

namespace Application.Users.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .WithMessage("FirstName cannot be empty.");

        RuleFor(c => c.LastName)
            .NotEmpty()
            .WithMessage("LastName cannot be empty.");

        RuleFor(c => c.Email)
            .EmailAddress()
            .WithMessage("Must be a valid email.");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty.")
            .MinimumLength(8)
            .WithMessage("Password must have minimum length of 8 character.");
    }
}
