using FluentValidation;

namespace HotDeskBooking.Commands.Locations.Delete;

public class DeleteLocationValidator : AbstractValidator<DeleteLocationCommand>
{
    public DeleteLocationValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");
    }
}
