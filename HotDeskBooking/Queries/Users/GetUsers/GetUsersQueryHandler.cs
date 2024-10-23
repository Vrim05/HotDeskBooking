using HotDeskBooking.Models.Dto;
using HotDeskBooking.Services;
using MediatR;

namespace HotDeskBooking.Queries.Users.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IReadOnlyList<UserDto>>
{
    public readonly IUserService _userService;
    public GetUsersQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IReadOnlyList<UserDto>> Handle(GetUsersQuery query, CancellationToken ct)
        => await _userService.GetAllAsync(ct);
}
