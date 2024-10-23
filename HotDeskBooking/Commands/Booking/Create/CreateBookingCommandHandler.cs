using HotDeskBooking.Models.Responses;
using HotDeskBooking.Services;
using MediatR;


namespace HotDeskBooking.Commands.Booking.Create;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, CreateOrUpdateBookingResponse>
{
    public readonly IBookingService _bookingService;
    public CreateBookingCommandHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<CreateOrUpdateBookingResponse> Handle(CreateBookingCommand request, CancellationToken ct)
        => await _bookingService.CreateAsync(request, ct);
}
