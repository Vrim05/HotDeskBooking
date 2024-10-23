using HotDeskBooking.Models.Requests;
using HotDeskBooking.Models.Responses;
using MediatR;


namespace HotDeskBooking.Commands.Users.Create;

public record CreateUserCommand(CreateUserRequest CreateRequest) : IRequest<CreateUserResponse>;
