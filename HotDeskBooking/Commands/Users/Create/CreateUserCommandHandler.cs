using HotDeskBooking.Models.Responses;
using HotDeskBooking.Services;
using MediatR;


namespace HotDeskBooking.Commands.Users.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    public readonly IUserService _userService;
    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken ct)
        => await _userService.CreateAsync(request, ct);
}
