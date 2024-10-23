using FluentValidation;
using HotDeskBooking.Commands.Locations.Create;

namespace HotDeskBooking.Commands.User.Create;

public class CreateUserValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.CreateRequest.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot be longer than 50 characters.");

        RuleFor(x => x.CreateRequest.RoomNumber)
            .GreaterThan((short)0).WithMessage("Room number must be greater than 0.");

        RuleFor(x => x.CreateRequest.BuildNumber)
            .GreaterThan((short)0).WithMessage("Build number must be greater than 0.");
    }
}
