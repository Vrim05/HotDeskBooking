using HotDeskBooking.Models.Requests;
using HotDeskBooking.Models.Responses;
using MediatR;

namespace HotDeskBooking.Commands.Users.Login;

public record LoginCommand(LoginRequest LoginRequest) : IRequest<AuthenticationResponse>;
