using FluentValidation;

namespace HotDeskBooking.Commands.Users.Delete;

public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");
    }
}
