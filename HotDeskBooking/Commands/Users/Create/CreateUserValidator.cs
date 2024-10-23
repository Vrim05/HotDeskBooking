using FluentValidation;

namespace HotDeskBooking.Commands.Users.Create;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.CreateRequest.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.CreateRequest.Password)
            .MinimumLength(6).When(x => !string.IsNullOrEmpty(x.CreateRequest.Password))
            .WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.CreateRequest.RoleId)
            .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");
    }
}
