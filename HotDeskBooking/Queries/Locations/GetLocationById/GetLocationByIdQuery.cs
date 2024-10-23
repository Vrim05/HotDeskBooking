using HotDeskBooking.Models.Dto;
using MediatR;

namespace HotDeskBooking.Queries.Locations.GetLocationById;

public record GetLocationByIdQuery(int Id) : IRequest<LocationDto>;
