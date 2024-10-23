using HotDeskBooking.Models.Responses;
using HotDeskBooking.Services;
using MediatR;


namespace HotDeskBooking.Commands.Locations.Delete;

public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, StandardResponse>
{
    public readonly ILocationService _locationService;
    public DeleteLocationCommandHandler(ILocationService locationService)
    {
        _locationService = locationService;
    }

    public async Task<StandardResponse> Handle(DeleteLocationCommand request, CancellationToken ct)
        => await _locationService.DeleteAsync(request, ct);
}
