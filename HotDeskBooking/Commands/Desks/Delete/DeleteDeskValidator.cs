using FluentValidation;

namespace HotDeskBooking.Commands.Desks.Delete;

public class DeleteDeskValidator : AbstractValidator<DeleteDeskCommand>
{
    public DeleteDeskValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");
    }
}
