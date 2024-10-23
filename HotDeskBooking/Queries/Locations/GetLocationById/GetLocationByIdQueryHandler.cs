using HotDeskBooking.Models.Dto;
using HotDeskBooking.Services;
using MediatR;

namespace HotDeskBooking.Queries.Locations.GetLocationById;

public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, LocationDto>
{
    public readonly ILocationService _locationService;
    public GetLocationByIdQueryHandler(ILocationService locationService)
    {
        _locationService = locationService;
    }

    public async Task<LocationDto> Handle(GetLocationByIdQuery query, CancellationToken ct)
        => await _locationService.GetByIdAsync(query, ct);
}
