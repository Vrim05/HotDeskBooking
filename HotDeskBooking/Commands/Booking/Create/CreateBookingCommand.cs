using HotDeskBooking.Models.Requests;
using HotDeskBooking.Models.Responses;
using MediatR;


namespace HotDeskBooking.Commands.Booking.Create;

public record CreateBookingCommand(CreateBookingRequest BookingRequest) : IRequest<CreateOrUpdateBookingResponse>;
