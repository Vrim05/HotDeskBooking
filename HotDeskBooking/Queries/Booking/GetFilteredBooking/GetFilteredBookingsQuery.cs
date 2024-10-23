using HotDeskBooking.Models.Dto;
using MediatR;

namespace HotDeskBooking.Queries.Booking.GetFilteredBooking;

public record GetFilteredBookingsQuery : IRequest<IReadOnlyList<BookingDto>>
{
    public int? UserId { get; init; }
    public int? DeskId { get; init; }
    public int? LocationId { get; init; }
}
