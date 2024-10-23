using HotDeskBooking.Models.Responses;
using MediatR;


namespace HotDeskBooking.Commands.Booking.Delete;

public record DeleteBookingCommand(int Id) : IRequest<StandardResponse>;
