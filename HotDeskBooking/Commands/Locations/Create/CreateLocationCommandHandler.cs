using HotDeskBooking.Models.Responses;
using HotDeskBooking.Services;
using MediatR;


namespace HotDeskBooking.Commands.Locations.Create;

public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, CreateLocationResponse>
{
    public readonly ILocationService _locationService;
    public CreateLocationCommandHandler(ILocationService locationService)
    {
        _locationService = locationService;
    }

    public async Task<CreateLocationResponse> Handle(CreateLocationCommand request, CancellationToken ct)
        => await _locationService.CreateAsync(request, ct);
}
