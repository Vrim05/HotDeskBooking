using FluentValidation;

namespace HotDeskBooking.Commands.Booking.Update;

public class UpdateBookingValidator : AbstractValidator<UpdateBookingCommand>
{
    public UpdateBookingValidator()
    {
        RuleFor(x => x.BookingRequest.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");

        RuleFor(x => x.BookingRequest.NewDeskId)
            .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");

        RuleFor(x => x.BookingRequest.NewStartDay)
                   .NotEmpty().WithMessage("Start day is required.")
                   .Must(BeAValidDate).WithMessage("Invalid start day.")
                   .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Start day cannot be in the past.");

        RuleFor(x => x.BookingRequest.NewEndDate)
            .NotEmpty().WithMessage("End date is required.")
            .Must(BeAValidDate).WithMessage("Invalid end date.")
            .GreaterThan(x => x.BookingRequest.NewStartDay).WithMessage("End date must be after the start day.");
    }

    private bool BeAValidDate(DateTime date)
    {
        return date > DateTime.MinValue;
    }
}
