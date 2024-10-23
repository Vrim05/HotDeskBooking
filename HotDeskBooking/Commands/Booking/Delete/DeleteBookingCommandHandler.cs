using HotDeskBooking.Models.Responses;
using HotDeskBooking.Services;
using MediatR;


namespace HotDeskBooking.Commands.Booking.Delete;

public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand, StandardResponse>
{
    public readonly IBookingService _bookingService;
    public DeleteBookingCommandHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<StandardResponse> Handle(DeleteBookingCommand request, CancellationToken ct)
        => await _bookingService.DeleteAsync(request, ct);
}
