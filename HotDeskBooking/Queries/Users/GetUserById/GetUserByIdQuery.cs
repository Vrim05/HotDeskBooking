using HotDeskBooking.Models.Dto;
using MediatR;

namespace HotDeskBooking.Queries.Users.GetUserById;

public record GetUserByIdQuery(int Id) : IRequest<UserDto>;
