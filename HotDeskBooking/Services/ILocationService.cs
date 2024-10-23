using HotDeskBooking.Commands.Locations.Create;
using HotDeskBooking.Commands.Locations.Delete;
using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Responses;
using HotDeskBooking.Queries.Locations.GetLocationById;

namespace HotDeskBooking.Services;

public interface ILocationService
{
    Task<IReadOnlyList<LocationDto>> GetLocationsAsync(CancellationToken ct);
    Task<LocationDto> GetByIdAsync(GetLocationByIdQuery query, CancellationToken ct);
    Task<CreateLocationResponse> CreateAsync(CreateLocationCommand command, CancellationToken ct);
    Task<StandardResponse> DeleteAsync(DeleteLocationCommand command, CancellationToken ct);
}
