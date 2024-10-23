using FluentValidation;

namespace HotDeskBooking.Queries.Booking.GetBookingById;

public class GetBookingByIdValidator : AbstractValidator<GetBookingByIdQuery>
{
    public GetBookingByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
