using FluentValidation;

namespace Application.Users.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(c => c.Email)
            .EmailAddress()
            .WithMessage("Must be a valid email.");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty")
            .MinimumLength(8)
            .WithMessage("Password must have minimum length of 8 character.")
            .MaximumLength(16)
            .WithMessage("Password must have maximum length of 16 character.")
            .Matches(@"[A-Z]+")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+")
            .WithMessage("Password must contain at least one number.")
            .Matches(@"[\!\?\*\.]+")
            .WithMessage("Password must contain at least one (!? *.).");
            
    }
}
