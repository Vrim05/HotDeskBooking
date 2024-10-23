using HotDeskBooking.Models.Responses;
using HotDeskBooking.Services;
using MediatR;


namespace HotDeskBooking.Commands.Booking.Update;

public class CreateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, CreateOrUpdateBookingResponse>
{
    public readonly IBookingService _bookingService;
    public CreateBookingCommandHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<CreateOrUpdateBookingResponse> Handle(UpdateBookingCommand request, CancellationToken ct)
        => await _bookingService.UpdateAsync(request, ct);
}
