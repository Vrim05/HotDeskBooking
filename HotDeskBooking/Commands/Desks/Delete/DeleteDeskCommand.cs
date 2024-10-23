using HotDeskBooking.Models.Responses;
using MediatR;


namespace HotDeskBooking.Commands.Desks.Delete;

public record DeleteDeskCommand(int Id) : IRequest<StandardResponse>;
