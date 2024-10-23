using HotDeskBooking.Models.Dto;
using MediatR;

namespace HotDeskBooking.Queries.Booking.GetBookingById;

public record GetBookingByIdQuery(int Id) : IRequest<BookingDto>;
