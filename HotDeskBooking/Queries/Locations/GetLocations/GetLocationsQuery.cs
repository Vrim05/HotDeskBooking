using HotDeskBooking.Models.Dto;
using MediatR;

namespace HotDeskBooking.Queries.Locations.GetLocations;

public record GetLocationsQuery : IRequest<IReadOnlyList<LocationDto>>;
