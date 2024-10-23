using FluentValidation;

namespace HotDeskBooking.Commands.Desks.Create;

public class CreateDeskValidator : AbstractValidator<CreateDeskCommand>
{
    public CreateDeskValidator()
    {
        RuleFor(x => x.DeskRequest.LocationId)
            .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");
    }
}
