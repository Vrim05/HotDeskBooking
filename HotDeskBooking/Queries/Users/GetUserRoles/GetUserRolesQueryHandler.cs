using HotDeskBooking.Models.Dto;
using HotDeskBooking.Services;
using MediatR;

namespace HotDeskBooking.Queries.Users.GetUserRoles;

public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, IReadOnlyList<UserRoleDto>>
{
    public readonly IUserService _userService;
    public GetUserRolesQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IReadOnlyList<UserRoleDto>> Handle(GetUserRolesQuery query, CancellationToken ct)
        => await _userService.GetUserRolesAsync(ct);
}
