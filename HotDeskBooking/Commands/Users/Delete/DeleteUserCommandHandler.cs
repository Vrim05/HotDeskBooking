using HotDeskBooking.Models.Responses;
using HotDeskBooking.Services;
using MediatR;


namespace HotDeskBooking.Commands.Users.Delete;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, StandardResponse>
{
    public readonly IUserService _userService;
    public DeleteUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<StandardResponse> Handle(DeleteUserCommand request, CancellationToken ct)
        => await _userService.DeleteAsync(request, ct);
}
