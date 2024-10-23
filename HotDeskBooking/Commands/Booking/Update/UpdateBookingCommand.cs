using HotDeskBooking.Models.Requests;
using HotDeskBooking.Models.Responses;
using MediatR;


namespace HotDeskBooking.Commands.Booking.Update;

public record UpdateBookingCommand(UpdateBookingRequest BookingRequest) : IRequest<CreateOrUpdateBookingResponse>;
