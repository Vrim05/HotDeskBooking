using HotDeskBooking.Models.Dto;
using MediatR;

namespace HotDeskBooking.Queries.Users.GetUserRoles;

public record GetUserRolesQuery : IRequest<IReadOnlyList<UserRoleDto>>;
