using HotDeskBooking.Models.Requests;
using HotDeskBooking.Models.Responses;
using MediatR;


namespace HotDeskBooking.Commands.Desks.Create;

public record CreateDeskCommand(CreateDeskRequest DeskRequest) : IRequest<CreateDeskResponse>;
