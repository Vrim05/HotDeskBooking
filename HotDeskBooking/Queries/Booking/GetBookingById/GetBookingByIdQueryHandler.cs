using HotDeskBooking.Models.Dto;
using HotDeskBooking.Services;
using MediatR;

namespace HotDeskBooking.Queries.Booking.GetBookingById;

public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingDto>
{
    public readonly IBookingService _bookingService;
    public GetBookingByIdQueryHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<BookingDto> Handle(GetBookingByIdQuery query, CancellationToken ct)
        => await _bookingService.GetByIdAsync(query, ct);
}
