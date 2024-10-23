using HotDeskBooking.Models.Responses;
using MediatR;


namespace HotDeskBooking.Commands.Locations.Delete;

public record DeleteLocationCommand(int Id) : IRequest<StandardResponse>;
