using HotDeskBooking.Models.Responses;
using HotDeskBooking.Services;
using MediatR;

namespace HotDeskBooking.Commands.Users.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
{
    public readonly IUserService _userService;
    public LoginCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken ct)
        => await _userService.AuthenticateAsync(request, ct);
}