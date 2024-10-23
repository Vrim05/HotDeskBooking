using HotDeskBooking.Models.Dto;
using MediatR;

namespace HotDeskBooking.Queries.Users.GetUsers;

public record GetUsersQuery : IRequest<IReadOnlyList<UserDto>>;
