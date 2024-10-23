using FluentValidation;

namespace HotDeskBooking.Commands.Users.Login;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.LoginRequest.Email)
            .NotEmpty().WithMessage("Email is required.");

        RuleFor(x => x.LoginRequest.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
