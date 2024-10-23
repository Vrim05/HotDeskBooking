using HotDeskBooking.Models.Dto;
using HotDeskBooking.Services;
using MediatR;

namespace HotDeskBooking.Queries.Locations.GetLocations;

public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, IReadOnlyList<LocationDto>>
{
    public readonly ILocationService _locationService;
    public GetLocationsQueryHandler(ILocationService locationService)
    {
        _locationService = locationService;
    }

    public async Task<IReadOnlyList<LocationDto>> Handle(GetLocationsQuery query, CancellationToken ct)
        => await _locationService.GetLocationsAsync(ct);
}
