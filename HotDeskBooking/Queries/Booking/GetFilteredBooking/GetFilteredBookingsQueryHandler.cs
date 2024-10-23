using HotDeskBooking.Models.Dto;
using HotDeskBooking.Services;
using MediatR;

namespace HotDeskBooking.Queries.Booking.GetFilteredBooking;

public class GetFilteredBookingsQueryHandler : IRequestHandler<GetFilteredBookingsQuery, IReadOnlyList<BookingDto>>
{
    public readonly IBookingService _bookingService;
    public GetFilteredBookingsQueryHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<IReadOnlyList<BookingDto>> Handle(GetFilteredBookingsQuery query, CancellationToken ct)
        => await _bookingService.GetFilteredBookingsAsync(query, ct);
}
