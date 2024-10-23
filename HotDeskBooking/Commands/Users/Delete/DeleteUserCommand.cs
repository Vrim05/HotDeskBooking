using HotDeskBooking.Models.Responses;
using MediatR;


namespace HotDeskBooking.Commands.Users.Delete;

public record DeleteUserCommand(int Id) : IRequest<StandardResponse>;
