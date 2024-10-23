using HotDeskBooking.Models.Requests;
using HotDeskBooking.Models.Responses;
using MediatR;


namespace HotDeskBooking.Commands.Locations.Create;

public record CreateLocationCommand(CreateLocationRequest CreateRequest) : IRequest<CreateLocationResponse>;
