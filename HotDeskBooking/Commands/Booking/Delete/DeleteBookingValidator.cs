using FluentValidation;

namespace HotDeskBooking.Commands.Booking.Delete;

public class DeleteBookingValidator : AbstractValidator<DeleteBookingCommand>
{
    public DeleteBookingValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");
    }
}
