using FluentValidation;
using HotDeskBooking.Models.Requests;

namespace HotDeskBooking.Commands.Booking.Create;

public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.BookingRequest.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");

        RuleFor(x => x.BookingRequest.DeskId)
            .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");

        RuleFor(x => x.BookingRequest.StartDay)
            .NotEmpty().WithMessage("Start day is required.")
            .Must(BeAValidDate).WithMessage("Invalid start day.")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Start day cannot be in the past.");

        RuleFor(x => x.BookingRequest.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .Must(BeAValidDate).WithMessage("Invalid end date.")
            .GreaterThan(x => x.BookingRequest.StartDay).WithMessage("End date must be after the start day.");

        RuleFor(x => x.BookingRequest)
            .Must(BeWithinOneWeek).WithMessage("Booking cannot be longer than 7 days.");
    }

    private bool BeAValidDate(DateTime date)
    {
        return date > DateTime.MinValue;
    }

    private bool BeWithinOneWeek(CreateBookingRequest bookingRequest)
    {
        var duration = (bookingRequest.EndDate - bookingRequest.StartDay).TotalDays;
        return duration <= 7;
    }
}
